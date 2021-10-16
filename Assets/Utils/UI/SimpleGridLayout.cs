using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Utils.UI
{
	//this is a very basic layout just to help if the number of heroes changes.
	public class SimpleGridLayout : LayoutGroup {
		
		[SerializeField]
		private Vector2 Spacing;
		
	    public override void CalculateLayoutInputVertical() {
	        base.CalculateLayoutInputHorizontal();


			var childCount = transform.childCount;
			var rect = rectTransform.rect;
			
			float usableWidth = rect.width;
			float usableHeight = rect.height;

			GetRowsAndColumns(childCount, usableWidth, usableHeight, out int totalRows, out int totalColumns);

			float xSpacingCellDelta = (Spacing.x / totalColumns) * (totalColumns - 1);
			float ySpacingCellDelta = (Spacing.y / totalRows) * (totalRows - 1);
			
			float cellWidth = (usableWidth / totalColumns) - 
							xSpacingCellDelta - 
								(padding.left / (float) totalColumns) - 
									(padding.right / (float) totalColumns);
			
			
			float cellHeight = (usableHeight / totalRows) - 
								ySpacingCellDelta - 
									(padding.top / (float) totalRows) - 
										(padding.bottom / (float) totalRows);


			int currentColunm = 0;
			int currentRow = 0;
			
			for (int i = 0; i < childCount; i++) {
				var currentCellItem = rectChildren[i];

				currentRow = i / totalColumns;
				currentColunm = i % totalColumns;
				
				float xPos = (cellWidth * currentColunm) + (Spacing.x * currentColunm) + padding.left;
				float yPos = (cellHeight * currentRow) + (Spacing.y * currentRow) + padding.top;
				
				SetChildAlongAxis(currentCellItem, 0, xPos, cellWidth);
				SetChildAlongAxis(currentCellItem, 1, yPos, cellHeight);
			}
			
		}

		private static void GetRowsAndColumns(int number, float usableWidth, float usableHeight, out int rows, out int columns) {

			int bigger;
			int smaller;
			
			switch (number) {
				default:
					float squareRatio = Mathf.Sqrt(number);
					columns = Mathf.CeilToInt(squareRatio);
					rows = Mathf.CeilToInt(number / (float) columns);
					return;
				
				//exceptions that just look better than trying to go for a squared layout 
				case 5:
					smaller = 1;
					bigger = 5;
					break;
				case 8:
					smaller = 2;
					bigger = 4;
					break;
				case 10:
					bigger = 5;
					smaller = 2;
					break;
			}
			
			if (usableHeight > usableWidth) {
				rows = bigger;
				columns = smaller;
			} else {
				rows = smaller;
				columns = bigger;
			}

			
		}

		public override void SetLayoutHorizontal() {}

	    public override void SetLayoutVertical() {}
	}

}



