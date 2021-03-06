﻿using LibToyBot.Outcomes;
using LibToyBot.Spatial;

namespace LibToyBot.Movement
{
    public interface IMovementProcessor
    {
        /// <summary>
        /// <para>
        ///     Attempts to move the robot forward one place, based on the current position and orientation of the robot
        /// </para>
        /// </summary>
        ActionOutcome Move();
        ActionOutcome Turn(Direction direction);

        ActionOutcome Place(in int xPosition, in int yPosition, Orientation orientation);
    }
}