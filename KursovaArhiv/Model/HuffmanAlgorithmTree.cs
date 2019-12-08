using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KursovaArhiv.Model
{
	[Serializable]
	internal class HuffmanAlgorithmTree
	{
		[Serializable]
		internal class Node
		{
			public char Symbol { get; set; }
			public long Freq { get; set; }
			public Node Right { get; set; }
			public Node Left { get; set; }
			
			public List<bool> Movement(char symbol, List<bool> data)
			{
				if (Right == null && Left == null)
				{
					return symbol.Equals(Symbol) ? data : null;
				}

				List<bool> left = null;
				List<bool> right = null;

				if (Left != null)
				{
					var leftPath = new List<bool>();
					leftPath.AddRange(data);
					leftPath.Add(false);

					left = Left.Movement(symbol, leftPath);
				}

				if (Right != null)
				{
					var rightPath = new List<bool>();
					rightPath.AddRange(data);
					rightPath.Add(true);

					right = Right.Movement(symbol, rightPath);
				}

				return left ?? right;
			}
		}

		[Serializable]
		private class FreqAndCode
		{
			public long Frequencies { get; }
			public bool[] Code { get; set; }

			public FreqAndCode(long freq, bool[] code)
			{
				Frequencies = freq;
				Code = code;
			}
		}

		private Node _root = new Node();
		private Node _currentRoot;
		private readonly List<Node> _nodes = new List<Node>();
		private readonly Dictionary<char, FreqAndCode> _frequencies = new Dictionary<char, FreqAndCode>();
		
		public void Build(Dictionary<char, long> symbols)
		{
			foreach (var symbol in symbols)
			{
				_frequencies.Add(symbol.Key, new FreqAndCode(symbol.Value, null));
			}
			
			SortTree();
		}
		
		private void SortTree()
		{
			foreach (var symbol in _frequencies)
			{
				_nodes.Add(new Node()
				{
					Symbol = symbol.Key,
					Freq = symbol.Value.Frequencies
				});
			}

			while (_nodes.Count > 1)
			{
				var orderedNodes = _nodes.OrderBy(node => node.Freq).ToList();

				if (orderedNodes.Count >= 2)
				{
					var taken = orderedNodes.Take(2).ToList();

					var parent = new Node()
					{
						Symbol = '*',
						Freq = taken[0].Freq + taken[1].Freq,
						Left = taken[0],
						Right = taken[1]
					};

					_nodes.Remove(taken[0]);
					_nodes.Remove(taken[1]);
					_nodes.Add(parent);
				}

				_currentRoot = _root = _nodes.FirstOrDefault();
			}
		}
		
		public List<bool> Encode(IEnumerable<byte> sourceStr)
		{
			var encodedSource = new List<bool>();

			foreach (char symbol in sourceStr)
			{
				if (_frequencies[symbol].Code == null)
				{
					_frequencies[symbol].Code = _root.Movement(symbol, new List<bool>()).ToArray();
				}

				encodedSource.AddRange(_frequencies[symbol].Code);
			}

			return encodedSource;
		}
		
		public StringBuilder Decode(byte[] bytes, long fileSize)
		{
			var bits = new BitArray(bytes);
			var decoded = new StringBuilder();
			var rangeOut = 0;

			if (fileSize <= 0)
			{
				rangeOut = bytes[bytes.Length - 1] + 8;
			}

			for (var i = 0; i < bits.Length - rangeOut; ++i)
			{
				if (bits[i])
				{
					if (_currentRoot.Right != null)
						_currentRoot = _currentRoot.Right;
				}
				else
				{
					if (_currentRoot.Left != null)
						_currentRoot = _currentRoot.Left;
				}

				if (!IsLeaf(_currentRoot)) continue;
				
				decoded.Append(_currentRoot.Symbol);
				_currentRoot = _root;
			}

			if (fileSize <= 0)
			{
				_currentRoot = _root;
			}

			return decoded;
		}
		
		private static bool IsLeaf(Node node)
		{
			return node.Left == null && node.Right == null;
		}
	}
}
