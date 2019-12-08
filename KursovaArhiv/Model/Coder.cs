using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using KursovaArhiv.Classes;

namespace KursovaArhiv.Model
{
	enum MAX_VALUE : int
	{
		BLOCK_SIZE = 256
	}

	internal class Coder
	{
		private HuffmanAlgorithmTree _tree;
		
		public void ZipHuffman(string path, string fileName, params string[] files)
		{
			_tree = new HuffmanAlgorithmTree();
			
			using (var writer = new BinaryWriter(File.OpenWrite(path + "\\" + fileName + ".hz"), Encoding.Default))
			{
				var onlyFiles = GetOnlyFiles(files);
				writer.Write(BitConverter.GetBytes(onlyFiles.Count));

				ExecuteParams.Max = onlyFiles.Count;
				ExecuteParams.CurrentValue = 0;
				ExecuteParams.Start = true;

				ExecuteParams.Str ="Write tree...";
				WriteTree(writer, BuildTree(onlyFiles.ToArray()));
				ExecuteParams.Str += "---OK\n";
				
				foreach (var file in files)
				{
					CreateEncodeFiles(file, writer);
				}

				ExecuteParams.Str += "End encoding\n";
				System.Threading.Thread.Sleep(1000);
				ExecuteParams.Start = false;
			}
		}

		public void UnpackHuffman(string path, string file)
		{
			using (var fs = new FileStream(file, FileMode.Open))
			using (var reader = new BinaryReader(fs, Encoding.Default))
			{
				ExecuteParams.Max =  BitConverter.ToInt32(reader.ReadBytes(4), 0);
				ExecuteParams.CurrentValue = 0;
				ExecuteParams.Start = true;

				var str = Encoding.Default.GetString(reader.ReadBytes(4));
				if (str.Equals("tjat"))
				{
					ExecuteParams.Str = "Read tree...  ";
					ReadTree(reader, BitConverter.ToInt32(reader.ReadBytes(8), 0));
					ExecuteParams.Str += "---OK\n";
			
					var dirName = TakeFileName(file).Split('.')[0];
					path += "\\" + dirName;
					Directory.CreateDirectory(path);

					CreateDecodeFiles(reader, path);
				}
				else
				{
					ExecuteParams.Str = "Can't find tree, execute exit\n";
				}

				ExecuteParams.Str += "End decoding\n";
				System.Threading.Thread.Sleep(1000);
				ExecuteParams.Start = false;
			}
		}
	
		private static List<string> GetOnlyFiles(IEnumerable<string> files)
		{
			var onlyFiles = new List<string>();

			foreach (var file in files)
			{
				if (!Directory.Exists(file))
				{
					onlyFiles.Add(file);
				}
				else
				{
					onlyFiles.AddRange(Directory.GetFiles(file));
					onlyFiles.AddRange(GetOnlyFiles(Directory.GetDirectories(file)));
				}
			}

			return onlyFiles;
		}
		
		private string BuildTree(IEnumerable<string> files)
		{
			var fileDicts = files.AsParallel().Select(CalcSymbolFreq);
			
			var res = (
				from dict in fileDicts 
				from symbol in dict 
				group symbol by symbol.Key into obj 
				select new {Name = obj.Key, Count = obj.Sum(item => item.Value)}
				).ToDictionary(item => item.Name, item => item.Count);

			_tree.Build(res);

			var treeName = $"{Guid.NewGuid()}.bin";
			using (Stream fStream = new FileStream(treeName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				new BinaryFormatter().Serialize(fStream, _tree);
			}

			return treeName;
		}

		private Dictionary<char, long> CalcSymbolFreq(string fileName)
		{
			var symbolFreq = new Dictionary<char, long>();
			
			using (var fs = new FileStream(fileName, FileMode.Open))
			using (var reader = new BinaryReader(fs))
			{
				var fileSize = reader.BaseStream.Length;

				while (fileSize > 0)
				{
					WriteInBuffer(out var buff, ref fileSize, reader);
					
					foreach (char symbol in buff)
					{
						if (!symbolFreq.ContainsKey(symbol))
							symbolFreq.Add(symbol, 0);
						symbolFreq[symbol]++;
					}
				}
			}

			return symbolFreq;
		}
		
		private static void WriteTree(BinaryWriter writer, string treeFileName)
		{
			writer.Write(Encoding.Default.GetBytes("tjat"));
			writer.Write(BitConverter.GetBytes(new FileInfo(treeFileName).Length));
			writer.Write(File.ReadAllBytes(treeFileName));
			File.Delete(treeFileName);
		}

		private void ReadTree(BinaryReader reader, int size)
		{
			var treeName = $"{Guid.NewGuid().ToString()}.bin";

			using (var fs = new FileStream(treeName, FileMode.CreateNew))
			{
				fs.Write(reader.ReadBytes(size), 0, size);
				fs.Position = 0;
				_tree = new BinaryFormatter().Deserialize(fs) as HuffmanAlgorithmTree;
			}

			File.Delete(treeName);
		}
		
		private static string TakeFileName(string file)
		{
			var filePath = file.Split('\\');

			return filePath[filePath.Length - 1];
		}
		
		private static void WriteInBuffer(out byte[] buffer, ref long length, BinaryReader reader)
		{
			buffer = CreateByteArray(length);
			reader.Read(buffer, 0, buffer.Length);
			length -= buffer.Length;
		}
		
		private static byte[] CreateByteArray(long size)
		{
			return size < (long)MAX_VALUE.BLOCK_SIZE ? new byte[size] : new byte[(int)MAX_VALUE.BLOCK_SIZE];
		}
		
		private void CreateEncodeFiles(string file, BinaryWriter writer)
		{
			if (!Directory.Exists(file))
			{
				ExecuteParams.Str += "Encode file: " + TakeFileName(file) + " ...  ";
				EncodeFile(file, writer);
				ExecuteParams.Str += "---OK\n";
				ExecuteParams.CurrentValue++;
			}

			else
			{
				ExecuteParams.Str += "Encode dir: " + TakeFileName(file) + " ...  ";
				writer.Write(Encoding.Default.GetBytes("tjad"));
				writer.Write(BitConverter.GetBytes(TakeFileName(file).Length));
				writer.Write(Encoding.Default.GetBytes(TakeFileName(file)));
				ExecuteParams.Str += "---OK\n";

				var files = new List<string>(Directory.GetDirectories(file));
				files.AddRange(Directory.GetFiles(file));

				foreach (var f in files)
				{
					CreateEncodeFiles(f, writer);
				}

				writer.Write(Encoding.Default.GetBytes("tjed"));
			}
		}
		
		private void CreateDecodeFiles(BinaryReader reader, string dirName)
		{
			while (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				switch (Encoding.Default.GetString(reader.ReadBytes(4)))
				{
					case "tjad":
						
						var dirNameSize = BitConverter.ToInt32(reader.ReadBytes(4), 0);
						var currentDirName = dirName + "\\" + Encoding.Default.GetString(reader.ReadBytes(dirNameSize));

						Directory.CreateDirectory(currentDirName);
						ExecuteParams.Str += "Decode dir:  " + TakeFileName(currentDirName) + "  ---OK\n";

						CreateDecodeFiles(reader, currentDirName);
						break;

					case "tjed":
						return;

					case "tjaf":
						var fileNameSize = BitConverter.ToInt32(reader.ReadBytes(4), 0);
						var fileName = Encoding.Default.GetString(reader.ReadBytes(fileNameSize));
						var fileSize = BitConverter.ToInt64(reader.ReadBytes(8), 0);

						ExecuteParams.Str += "Decode file:  " + fileName;
						DecodeFile(reader, dirName + "\\" + fileName, ref fileSize);
						ExecuteParams.Str += " ---OK\n";
						ExecuteParams.CurrentValue++;

						break;
				}
			}
		}
		
		private void EncodeFile(string file, BinaryWriter writer)
		{
			writer.Write(Encoding.Default.GetBytes("tjaf"));
			writer.Write(BitConverter.GetBytes(TakeFileName(file).Length));
			writer.Write(Encoding.Default.GetBytes(TakeFileName(file)));

			var encodeFileName = EncodeFile(file);  

			using (var fs = new FileStream(encodeFileName, FileMode.Open))
			using (var reader = new BinaryReader(fs))
			{
				var fileSize = reader.BaseStream.Length;
				writer.Write(BitConverter.GetBytes(fileSize));
				
				while (fileSize > 0)
				{
					WriteInBuffer(out var buff, ref fileSize, reader);
					writer.Write(buff);
				}
			}

			File.Delete(encodeFileName);
		}
		
		private string EncodeFile(string file)
		{
			var encodeFileName = $"{Guid.NewGuid().ToString()}.bin";

			using (var fs = new FileStream(file, FileMode.Open))
			using (var reader = new BinaryReader(fs))
			using (var writer = new BinaryWriter(File.OpenWrite(encodeFileName), Encoding.UTF8))
			{
				List<bool> notFullByte = null;
				var fileSize = reader.BaseStream.Length;

				while (fileSize > 0)
				{
					WriteInBuffer(out byte[] buff, ref fileSize, reader);
					notFullByte = BitArrayToByteArray(_tree.Encode(buff), out byte[] bytes, notFullByte);
					writer.Write(bytes);
				}

				if (notFullByte != null)
				{
					writer.Write(MoveNotFullBitsToLeft(notFullByte));
				}
			}

			return encodeFileName;
		}
		
		private void DecodeFile(BinaryReader reader, string fileName, ref long fileSize)
		{
			using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
			using (var bs = new BufferedStream(fs))
			using (TextWriter writer = new StreamWriter(bs))
			{
				while (fileSize > 0)
				{
					WriteInBuffer(out byte[] buff, ref fileSize, reader);
					writer.Write(_tree.Decode(buff, fileSize));
				}
			}
		}
		private static List<bool> BitArrayToByteArray(List<bool> bits, out byte[] outBytes, List<bool> lastNotFullByte)
		{
			if (lastNotFullByte != null)
			{
				lastNotFullByte.AddRange(bits);
				bits = lastNotFullByte;
			}
			
			var outRange = bits.Count % 8;

			outBytes = PuckBitsInByte(bits.ToArray(), outRange);

			if (outRange == 0) return null;
			var notFullByte = new List<bool>();

			for (var i = bits.Count - outRange; i < bits.Count; ++i)
			{
				notFullByte.Add(bits[i]);
			}

			return notFullByte;

		}
		private static byte[] PuckBitsInByte(IReadOnlyList<bool> boolArr, int outRange)
		{
			var bits = new BitArray(boolArr.Count - outRange);

			for (var i = 0; i < bits.Count; i++)
			{
				bits[i] = boolArr[i];
			}

			var bytes = new byte[bits.Length / 8];
			bits.CopyTo(bytes, 0);

			return bytes;
		}
		
		private static byte[] MoveNotFullBitsToLeft(IReadOnlyList<bool> bits)
		{
			var bytes = new byte[2];
			var bitArray = new BitArray(8);

			for (var i = 0; i < bits.Count; ++i)
			{
				bitArray[i] = bits[i];
			}

			bitArray.CopyTo(bytes, 0);
			bytes[1] = (byte)(8 - bits.Count);

			return bytes;
		}
	}
}
