using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    public static class Constants
    {
        public static readonly int Rows = 8;										//Size of Board: # of Rows
        public static readonly int Columns = 8;										//Size of Board: # of Columns
        public static readonly float AnimationDuration =  0.10f;					//Minimum duration of each Animation
        public static readonly float MoveAnimationMinDuration = 0.065f;				//Minimum duration of each Block Swap
        public static readonly float ExplosionDuration = 0.3f;						//Duration of Explosion when block is destroyed
        public static readonly float OpacityAnimationFrameDelay = 0.05f;			//
        public static readonly int MinimumMatches = 3;								//Minimum number of blocks needed to make a match
        public static readonly int MinimumMatchesForBonus = 4;						//Minimum number of blocks needed to warrant a bonus piece
        public static readonly int Match3Score = 3;									//Default point amount for basic match
        public static readonly int SubsequentMatchScore = 9;						//Bonus points for waterfall matches
    }

   

