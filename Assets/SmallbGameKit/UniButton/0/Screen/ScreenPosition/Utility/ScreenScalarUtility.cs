using UnityEngine;
using System.Collections;

namespace UniButton
{
	public enum EScreenScalarUnit
	{
		Pixel,
		ScreenPercent,
		ScreenHeightPercent,
		ScreenWidthPercent,
		ScreenMaxDimensionPercent,
		ScreenMinDimensionPercent
	}
	
	public enum EScreenScalarUseOrientation
	{
		Height,
		Width
	}
	
	public static class ScreenScalarUtility
	{
		const float mc_fPercentInspectorUnit = 0.01f;
		
		public static float GetScreenDistanceInPixel(float a_fScalarValue, EScreenScalarUnit a_eScalarUnit, EScreenScalarUseOrientation a_eOrientation, Camera a_rCamera)
		{
			return GetScreenDistanceInPixel(a_fScalarValue, a_eScalarUnit, a_eOrientation, a_rCamera.pixelRect);
		}
		
		public static float GetScreenDistanceInPixel(float a_fScalarValue, EScreenScalarUnit a_eScalarUnit, EScreenScalarUseOrientation a_eOrientation, Rect a_oNormalizedViewportRectangle)
		{
			float fScreenDistance;
			
			fScreenDistance = 0.0f;
			switch(a_eScalarUnit)
			{	
				case EScreenScalarUnit.Pixel:
				{
					fScreenDistance = a_fScalarValue;
				}
				break;
				
				case EScreenScalarUnit.ScreenPercent:
				{
					switch(a_eOrientation)
					{	
						case EScreenScalarUseOrientation.Height:
						{
							fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.height;
						}
						break;
					
						case EScreenScalarUseOrientation.Width:
						{
							fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.width;
						}
						break;
					}
				}
				break;
				
				case EScreenScalarUnit.ScreenHeightPercent:
				{
					fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.height;
				}
				break;
				
				case EScreenScalarUnit.ScreenWidthPercent:
				{
					fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.width;
				}
				break;
				
				case EScreenScalarUnit.ScreenMaxDimensionPercent:
				{
					if(a_oNormalizedViewportRectangle.height > a_oNormalizedViewportRectangle.width)
					{
						fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.height;
					}
					else
					{
						fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.width;
					}
				}
				break;
				
				case EScreenScalarUnit.ScreenMinDimensionPercent:
				{
					if(a_oNormalizedViewportRectangle.height < a_oNormalizedViewportRectangle.width)
					{
						fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.height;
					}
					else
					{
						fScreenDistance = a_fScalarValue * a_oNormalizedViewportRectangle.width;
					}
				}
				break;
			}
			
			if(IsUnitInPercent(a_eScalarUnit))
			{
				fScreenDistance = ConvertPercentFromDecimalToInspectorUnit(fScreenDistance);
			}
			
			return fScreenDistance;
		}
		
		public static float GetScreenPointCoordinateInPixel(float a_fScalarValue, EScreenScalarUnit a_eScalarUnit, EScreenScalarUseOrientation a_eOrientation, EScreenDocking a_eDocking, Camera a_rCamera)
		{
			return GetScreenPointCoordinateInPixel(a_fScalarValue, a_eScalarUnit, a_eOrientation, a_eDocking, a_rCamera.pixelRect);
		}
		
		public static float GetScreenPointCoordinateInPixel(float a_fScalarValue, EScreenScalarUnit a_eScalarUnit, EScreenScalarUseOrientation a_eOrientation, 
			EScreenDocking a_eDocking, Rect a_oNormalizedViewportRectangle)
		{
			float fScreenPointCoordinate = 0.0f;
			float fScreenDistance = GetScreenDistanceInPixel(a_fScalarValue, a_eScalarUnit, a_eOrientation, a_oNormalizedViewportRectangle);
			fScreenPointCoordinate = ScreenDockingUtility.ComputeDockedScreenPoint(fScreenDistance, a_eOrientation, a_eDocking, a_oNormalizedViewportRectangle);
			
			return fScreenPointCoordinate;
		}
		
		static bool IsUnitInPercent(EScreenScalarUnit a_eUnit)
		{
			return 	a_eUnit == EScreenScalarUnit.ScreenPercent ||
					a_eUnit == EScreenScalarUnit.ScreenHeightPercent ||
					a_eUnit == EScreenScalarUnit.ScreenWidthPercent ||
					a_eUnit == EScreenScalarUnit.ScreenMaxDimensionPercent ||
					a_eUnit == EScreenScalarUnit.ScreenMinDimensionPercent;
		}
		
		static float ConvertPercentFromDecimalToInspectorUnit(float a_fPercentInDecimal)
		{
			return a_fPercentInDecimal * mc_fPercentInspectorUnit;
		}
	}
}