using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGrid : LayoutGroup
{
    public enum FitType
    {
        Width,  // match screen width
        Height, // match screen height
    }

    public FitType m_fitType;

    public int m_rows;
    public int m_columns;
    public Vector2 m_cellSize;
    public Vector2 m_spacing;
    float m_parentWidth;
    float m_parentHeight;
    public float m_ratio;


    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        m_parentWidth = rectTransform.rect.width;
        m_parentHeight = rectTransform.rect.height;
        m_ratio = m_parentWidth / m_parentHeight;

        if (m_ratio < 1.7f)
        {
            m_fitType = FitType.Width;
        }
        else
        {
            m_fitType = FitType.Height;
        }

        //grid with same row and column count
        float sqrRt = Mathf.Sqrt(rectChildren.Count);
        m_rows = Mathf.CeilToInt(sqrRt);
        m_columns = m_rows;

        if (m_fitType == FitType.Width) //keeps column count and adjusts rows
        {
            m_columns = Mathf.CeilToInt(rectChildren.Count / (float)m_columns);
        }
        if (m_fitType == FitType.Height) //keeps row count and adjusts columns
        {
            m_rows = Mathf.CeilToInt(rectChildren.Count / (float)m_rows);
        }

        float cellWidth = (m_parentWidth / (float)m_columns) - ((m_spacing.x / ((float)m_columns)) * (m_columns - 1)) - (padding.left / (float)m_columns) - (padding.right / (float)m_columns);
        float cellHeight = (m_parentHeight / (float)m_rows) - ((m_spacing.y / ((float)m_rows)) * (m_rows - 1)) - (padding.top / (float)m_rows) - (padding.bottom / (float)m_rows);

        m_cellSize.x = cellWidth;
        m_cellSize.y = cellHeight;


        int bottomCount = m_columns - ((m_rows * m_columns) % rectChildren.Count);

        int columnCount = 0;
        int rowCount = 0;
        var xPos = 0f;
        var yPos = 0f;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / m_columns;
            columnCount = i % m_columns;

            var item = rectChildren[i];

            if ((bottomCount != m_columns && rowCount == m_rows - 1))
            {
                float offset;
                if (bottomCount == 1)
                {
                    offset = m_parentWidth / 2 - m_cellSize.x / 2;
                }
                else
                {
                    offset = (m_parentWidth / m_columns) / 2 + (m_spacing.x * columnCount) + padding.left;
                }

                xPos = (m_cellSize.x * columnCount) + offset;
            }
            else
            {
                xPos = (m_cellSize.x * columnCount) + (m_spacing.x * columnCount) + padding.left;
            }
            yPos = (m_cellSize.y * rowCount) + (m_spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, m_cellSize.x);
            SetChildAlongAxis(item, 1, yPos, m_cellSize.y);
        }
    }

    public override void CalculateLayoutInputVertical() { }

    public override void SetLayoutHorizontal() { }

    public override void SetLayoutVertical() { }
}