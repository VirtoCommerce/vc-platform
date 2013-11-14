using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	/// <summary> Panel for laying out content id a tiled fashion. </summary>
	/// <seealso cref="System.Windows.Controls.Panel"/>
	public class TilePanel : Panel
	{
		public enum OrientationCategories
		{
			Vertical = 0,
			Horizontal = 1,
		}
		
		public static readonly DependencyProperty OrientationProperty =
		DependencyProperty.Register("Orientation", typeof(OrientationCategories), typeof(TilePanel), new PropertyMetadata(OrientationCategories.Vertical, OnOrientationChanged));

		public OrientationCategories Orientation  // here I get an error "Expected class, delegate, enum, interface or struct"
			{
				get { return (OrientationCategories)GetValue(OrientationProperty); }
				set
				{
					SetValue(OrientationProperty, value);
				}
			}

		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Panel)d).InvalidateMeasure();
		}

		public static readonly DependencyProperty AlignmentBoundaryProperty =
		DependencyProperty.Register("AlignmentBoundary", typeof(double), typeof(TilePanel), new PropertyMetadata(0.0, OnAlignmentBoundaryChanged));

		public double AlignmentBoundary
		{
			get { return (double)GetValue(AlignmentBoundaryProperty); }
			set
			{
				SetValue(AlignmentBoundaryProperty, value);
			}
		}

		private static void OnAlignmentBoundaryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Panel)d).InvalidateMeasure();
		}

		public static readonly DependencyProperty AlignOnLesserEdgeProperty =
		DependencyProperty.Register("AlignOnLesserEdge", typeof(bool), typeof(TilePanel), new PropertyMetadata(false, OnAlignOnLesserEdgeChanged));

		public bool AlignOnLesserEdge  // here I get an error "Expected class, delegate, enum, interface or struct"
		{
			get { return (bool)GetValue(AlignOnLesserEdgeProperty); }
			set
			{
				SetValue(AlignOnLesserEdgeProperty, value);
			}
		}

		private static void OnAlignOnLesserEdgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Panel)d).InvalidateMeasure();
		}

		/// <summary> A utility class used to tile blocks into a vertical stack of blocks. </summary>
		private class SpaceManager
		{
			private enum CompareActions
			{
				None,
				Delete,
				Split
			}

			/// <summary> Edge item - an item that defines a level at which a new item can be added. </summary>
			private class EdgeItem
			{
				/// <summary> Constructor. </summary>
				/// <param name="edgePosition1">  The left body (or top body if transposed). </param>
				/// <param name="edgePosition2"> The right body (or bottom body if transposed). </param>
				/// <param name="level">	 The level. </param>
				/// <param name="edgeLimit1">  The left wing  (or top wing if transposed). </param>
				/// <param name="edgeLimit2"> The right wing  (or bottom body if transposed). </param>
				public EdgeItem(double edgePosition1, double edgePosition2, double level, double edgeLimit1, double edgeLimit2)
				{
					Level = level;
					EdgePosition1 = edgePosition1;
					EdgePosition2 = edgePosition2;
					EdgeLimit1 = edgeLimit1;
					EdgeLimit2 = edgeLimit2;
				}

				/// <summary> Copy constructor. </summary>
				/// <param name="item"> The item. </param>
				public EdgeItem(EdgeItem item)
				{
					Copy(item);
				}

				/// <summary> Copies the given item. </summary>
				/// <param name="item"> The item. </param>
				public void Copy(EdgeItem item)
				{
					Level = item.Level;
					EdgePosition1 = item.EdgePosition1;
					EdgePosition2 = item.EdgePosition2;
					EdgeLimit1 = item.EdgeLimit1;
					EdgeLimit2 = item.EdgeLimit2;
				}

				/// <summary> Compare and adjust this item with a higher level item. </summary>
				/// <param name="item"> The item to compare against. </param>
				/// <returns> true if this item can be removed otherwise false. </returns>
				public CompareActions CompareAndAdjustBody(EdgeItem item)
				{
					if (item.Level < Level)
					{
						// item has a lower level so we don't need to adjust
						return CompareActions.None;
					}
					if((item.EdgePosition2 < EdgePosition1) || (item.EdgePosition1 > EdgePosition2))
					{
						// No overlap
						return CompareActions.None;
					}
					if ((item.EdgePosition2 >= EdgePosition2) && (item.EdgePosition1 <= EdgePosition1))
					{
						// completely overlapped
						return CompareActions.Delete;
					}
					// partially overlapped so reduce size to only non overlapped
					if((EdgePosition1 < item.EdgePosition1) && (EdgePosition2 > item.EdgePosition2))
					{
						return CompareActions.Split;
					}
					if (EdgePosition1 < item.EdgePosition1)
					{
						EdgePosition2 = item.EdgePosition1;
					}
					else
					{
						EdgePosition1 = item.EdgePosition2;
					}
					// if size is too small, mark it for delete
					return (EdgePosition2 - EdgePosition1 < 0) ? CompareActions.Delete : CompareActions.None;
				}

				/// <summary> Compare and adjust this item with a higher level item. </summary>
				/// <param name="item"> The item to compare against. </param>
				/// <returns> true if this item can be removed otherwise false. </returns>
				public void CompareAndAdjustWing(EdgeItem item)
				{
					if (item.Level <= Level)
					{
						// item has a lower level so we don't need to adjust
						return;
					}
					if ((item.EdgePosition2 < EdgeLimit1) || (item.EdgePosition1 > EdgeLimit2))
					{
						// No overlap
						return;
					}
					if ((item.EdgePosition2 >= EdgeLimit2) && (item.EdgePosition1 <= EdgeLimit1))
					{
						// completely overlapped
						Debug.Assert(false, "This Item should have been deleted already");
						return;
					}
					// partially overlapped so reduce size to only non overlapped
					if ((EdgeLimit1 < item.EdgePosition1) && (EdgeLimit2 > item.EdgePosition2))
					{
						if(EdgePosition1 > item.EdgePosition1)
						{
							EdgeLimit1 = item.EdgePosition2;
						}
						else
						{
							EdgeLimit2 = item.EdgePosition1;
						}
						return;
					}
					if (EdgeLimit1 < item.EdgePosition1)
					{
						EdgeLimit2 = item.EdgePosition1;
					}
					else
					{
						EdgeLimit1 = item.EdgePosition2;
					}
				}

				/// <summary> Determine if we can be with the supplied item. </summary>
				/// <param name="edgeItem">			 The edge item to be merged with. </param>
				/// <param name="alignBoundary">	 The align boundary. </param>
				/// <param name="alignOnLesserEdge"> true to align on lesser edge. </param>
				/// <returns> true if we can be merged, false if not. </returns>
				public bool CanBeMerged(EdgeItem edgeItem, double alignBoundary, bool alignOnLesserEdge)
				{
					if ((Level - edgeItem.Level > 0) && (Level - edgeItem.Level < alignBoundary))
					{
						return ((edgeItem.EdgeLimit1 >= EdgeLimit1) && (edgeItem.EdgeLimit1 <= EdgeLimit2)) && ((edgeItem.EdgeLimit2 >= EdgeLimit1) && (edgeItem.EdgeLimit2 <= EdgeLimit2) &&
							((alignOnLesserEdge) || (EdgePosition1 < edgeItem.EdgePosition2)));
					}
					return false;
				}

				/// <summary> Gets or sets the vertical edge level. </summary>
				/// <value> The level. </value>
				public double Level { get; set; }

				/// <summary> Gets or sets the right body (or bottom body if transposed). </summary>
				/// <value> The right body  (or bottom body if transposed). </value>
				public double EdgePosition2 { get; set; }

				/// <summary> Gets or sets the left (or top body if transposed). </summary>
				/// <value> The left (or top body if transposed). </value>
				public double EdgePosition1 { get; set; }

				/// <summary> Gets or sets the right (or bottom wing if transposed). </summary>
				/// <value> The right  (or bottom wing if transposed). </value>
				public double EdgeLimit2 { get; set; }

				/// <summary> Gets or sets the left  (or top wing if transposed). </summary>
				/// <value> The left  (or top wing if transposed). </value>
				public double EdgeLimit1 { get; set; }

				/// <summary> Fit rectangle the rectangle into the edge space. </summary>
				/// <param name="blockWidth"> the width of the block to be fitted. </param>
				/// <param name="transposed"> true if transposed. </param>
				/// <returns> returns the top left position if fitted else null. </returns>
				public Point? FitBlock(double blockWidth, bool transposed)
				{
					if (blockWidth <= EdgeLimit2 - EdgeLimit1)
					{
						return transposed ? new Point(Level, EdgeLimit1) : new Point(EdgeLimit1, Level);
					}
					return null;
				}
			}

			/// <summary> Gets or sets the align limit. </summary>
			/// <value> The align limit. </value>
			public double AlignLimit { get; set; }

			/// <summary> Gets or sets a value indicating whether the align on lesser edge. </summary>
			/// <value> true if align on lesser edge, false if not. </value>
			public bool AlignOnLesserEdge { get; set; }

			/// <summary> The left stack position. </summary>
			private readonly double _areaLimit1;
			/// <summary> The right stack position. </summary>
			private readonly double _areaLimit2;
			/// <summary> The top stack position. </summary>
			private readonly double _minAreaLevel;

			/// <summary> true if transposed (Horizontal) else (Vertical). </summary>
			private readonly bool _transposed;

			/// <summary> List of linked block rectangles. </summary>
			private readonly LinkedList<Rect> _linkedList = new LinkedList<Rect>();
			/// <summary> List of edge items. </summary>
			private List<EdgeItem> _edgeItemList;
			/// <summary> Collection of edge items. </summary>
			private LinkedList<EdgeItem> _edgeItemCollection;

			/// <summary> Constructor. </summary>
			/// <param name="areaLimit2">   The right edge of the area (or bottom edge if transposed). </param>
			/// <param name="transposed">   true if transposed. </param>
			/// <param name="minAreaLevel"> (optional) the top edge of the area (or left edge if transposed). </param>
			/// <param name="areaLimit1">   (optional) the left edge of the area (or top edge if transposed. </param>
			public SpaceManager(double areaLimit2, bool transposed=false, double minAreaLevel=0, double areaLimit1=0)
			{
				_transposed = transposed;
				_areaLimit1 = areaLimit1;
				_areaLimit2 = areaLimit2;
				_minAreaLevel = minAreaLevel;
			}

			/// <summary> Check for overlap. </summary>
			/// <param name="index"> Zero-based index of the. </param>
			/// <param name="edges"> The edges. </param>
			/// <returns> true if it succeeds, false if it fails. </returns>
			private CompareActions CheckForOverlap(LinkedListNode<EdgeItem> index, List<EdgeItem> edges)
			{
				// compare the current item to the following items (should be greater level)
				LinkedListNode<EdgeItem> subIndex = index.Next;
				while(subIndex != null)
				{
					switch(index.Value.CompareAndAdjustBody(subIndex.Value))
					{
						// item is no longer neede so request it be deleted
						case CompareActions.Delete:
							return CompareActions.Delete;
						// item needs to be split into 2 items
						case CompareActions.Split:
							// compare split item sizes
							double w1 = subIndex.Value.EdgePosition1 - index.Value.EdgePosition1;
							double w2 = index.Value.EdgePosition2 - subIndex.Value.EdgePosition2;
							if((w1 < 0) && (w2 < 0))
							{
								// both too small, so delete item instead
								return CompareActions.Delete;
							}
							if(w1 < 0)
							{
								// ignore w1 and use only w2
								index.Value.EdgePosition1 = subIndex.Value.EdgePosition2;
							}
							else if(w2 < 0)
							{
								// ignore w2 and use only w1
								index.Value.EdgePosition2 = subIndex.Value.EdgePosition1;
							}
							else
							{
								// shortened w1 and insert new w2
								EdgeItem newItem = new EdgeItem(subIndex.Value.EdgePosition2, index.Value.EdgePosition2, index.Value.Level, _areaLimit1, _areaLimit2);
								index.Value.EdgePosition2 = subIndex.Value.EdgePosition1;
								index.List.AddAfter(index, newItem);
								int idx = edges.FindIndex(edge => index.Value == edge);
								edges.Insert(idx + 1, newItem);
							}
							break;
					}
					subIndex = subIndex.Next;
				}
				return CompareActions.None;
			}

			/// <summary> Expand size of the edge to it's nax size. </summary>
			/// <param name="index"> Zero-based index of the. </param>
			private void GetExpandedEdgeItem(LinkedListNode<EdgeItem> index)
			{
				LinkedListNode<EdgeItem> subIndex = index.Next;
				while (subIndex != null)
				{
					index.Value.CompareAndAdjustWing(subIndex.Value);
					subIndex = subIndex.Next;
				}
				subIndex = index.Previous;
				while (subIndex != null)
				{
					index.Value.CompareAndAdjustWing(subIndex.Value);
					subIndex = subIndex.Previous;
				}
			}

			/// <summary> Adds an edge to the edge list and recalculates edge collection. </summary>
			/// <param name="rect"> The rectangle. </param>
			private void AddEdgeToCollection(Rect rect)
			{
				_linkedList.AddLast(rect);
				if (_edgeItemList == null)
				{
					_edgeItemList = new List<EdgeItem>();
					_edgeItemList.Add(new EdgeItem(_areaLimit1, _areaLimit2, _minAreaLevel, _areaLimit1, _areaLimit2));
				}
				_edgeItemList.Add(_transposed ? new EdgeItem(rect.Top, rect.Bottom, rect.Right, _areaLimit1, _areaLimit2) : new EdgeItem(rect.Left, rect.Right, rect.Bottom, _areaLimit1, _areaLimit2));
				_edgeItemList = _edgeItemList.AsEnumerable().OrderBy(r => r.Level).ToList();

				// adjust item sizes if overlapped by item with greater edge, removing items that are too small or completely overlapped
				_edgeItemCollection = new LinkedList<EdgeItem>(_edgeItemList);
				LinkedListNode<EdgeItem> index = _edgeItemCollection.First;
				while (index != null)
				{
					if (CheckForOverlap(index, _edgeItemList) == CompareActions.Delete)
					{
						_edgeItemList.Remove(index.Value);
					}
					index = index.Next;
				}

				// calculate extents of each edge
				_edgeItemCollection = new LinkedList<EdgeItem>(_edgeItemList);
				index = _edgeItemCollection.First;
				while (index != null)
				{
					GetExpandedEdgeItem(index);
					index = index.Next;
				}

				// level off close edges
				index = _edgeItemCollection.First;
				while ((index != null)/* && (index.Next != null)*/)
				{
					List<EdgeItem> closeItems = _edgeItemCollection.Where(edge => index.Value.CanBeMerged(edge, AlignLimit, AlignOnLesserEdge)).ToList();
					foreach (var closeItem in closeItems)
					{
						closeItem.Level = index.Value.Level;
						closeItem.EdgeLimit1 = _areaLimit1;
						closeItem.EdgeLimit2 = _areaLimit2;
						var closeIndex = index.List.Find(closeItem);
						GetExpandedEdgeItem(closeIndex);
					}
					index = index.Next;
				}
			}

			/// <summary> Inserts an edge described by size. </summary>
			/// <param name="size"> The size. </param>
			/// <returns> . </returns>
			public Point InsertEdge(Size size)
			{
				Rect r;
				// handle first element
				if (_linkedList.Count == 0)
				{
					_edgeItemList = null;
					_edgeItemCollection = null;
					r = _transposed ? new Rect(new Point(_minAreaLevel, _areaLimit1), size) : new Rect(new Point(_areaLimit1, _minAreaLevel), size);
					AddEdgeToCollection(r);
					return r.TopLeft;
				}
				LinkedListNode<EdgeItem> index = _edgeItemCollection.First;
				while (index != null)
				{
					Point? pt = _transposed ? index.Value.FitBlock(size.Height, true) : index.Value.FitBlock(size.Width, false);
					if (pt.HasValue)
					{
						AddEdgeToCollection(new Rect(pt.Value, size));
						return pt.Value;
					}
					index = index.Next;
				}
				Size sz = BoundingArea();
				r = _transposed ? new Rect(new Point(sz.Width + _minAreaLevel, _areaLimit1), size) : new Rect(new Point(_areaLimit1, sz.Height + _minAreaLevel), size);
				AddEdgeToCollection(r);
				return r.TopLeft;
			}

			public Size BoundingArea()
			{
				if(_linkedList.Count == 0)
				{
					if(_transposed)
					{
						return new Size(_minAreaLevel, _areaLimit1);
					}
					return new Size(_areaLimit1, _minAreaLevel);
				}
				double right = _linkedList.AsEnumerable().Select(r => r.Right).Max();
				double bottom = _linkedList.AsEnumerable().Select(r => r.Bottom).Max();
				if (_transposed)
				{
					return new Size(right - _minAreaLevel, bottom - _areaLimit1);
				}
				return new Size(right - _areaLimit1, bottom - _minAreaLevel);
			}
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			SpaceManager spaceManager = (Orientation == OrientationCategories.Vertical) ? new SpaceManager(finalSize.Width, false) : new SpaceManager(finalSize.Height, true);
			spaceManager.AlignLimit = AlignmentBoundary;
			spaceManager.AlignOnLesserEdge = AlignOnLesserEdge;

			foreach (UIElement child in Children)
			{
				Point point = spaceManager.InsertEdge(child.DesiredSize);
				child.Arrange(new Rect(point.X, point.Y, child.DesiredSize.Width, child.DesiredSize.Height));
			}
			return spaceManager.BoundingArea();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			SpaceManager spaceManager = (Orientation == OrientationCategories.Vertical) ? new SpaceManager(availableSize.Width, false) : new SpaceManager(availableSize.Height, true);
			spaceManager.AlignLimit = AlignmentBoundary;
			spaceManager.AlignOnLesserEdge = AlignOnLesserEdge;

			foreach (UIElement child in Children)
			{
				child.Measure(availableSize);
				spaceManager.InsertEdge(child.DesiredSize);
			}
			Size size = spaceManager.BoundingArea();
			return new Size(double.IsPositiveInfinity(availableSize.Width) ? size.Width : availableSize.Width, double.IsPositiveInfinity(availableSize.Height) ? size.Height : availableSize.Height);
		}

	}
}
