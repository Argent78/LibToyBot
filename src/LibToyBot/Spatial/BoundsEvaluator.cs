﻿namespace LibToyBot
{
    internal class BoundsEvaluator : IBoundsEvaluator
    {
        private readonly Table _table;

        public BoundsEvaluator(Table table)
        {
            _table = table;
        }

        /// <summary>
        ///   <para>
        ///     Validates that the specified x &amp; y position are within the bounds of the table's horizontal and vertical axis.
        ///     Tennis rules apply; on the line is in.
        ///   </para>
        /// </summary>
        /// <param name="xPosition">The x position.</param>
        /// <param name="yPosition">The y position.</param>
        /// <returns></returns>
        public bool InBounds(in int xPosition, in int yPosition)
        {
            // the robot positions are zero-based indexes, while the table dimension are one-based
            // so need to decrement the table-axis to match the robot position
            return xPosition <= _table.HorizontalAxis-1 && xPosition >= 0 
                   && yPosition <= _table.VerticalAxis-1 && yPosition >= 0;
        }
    }
}
