using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Office.Interop.Excel;
using TigerGenerator.Logic.Models;
using ToolsPortable;

namespace TigerGenerator.Logic.Helpers
{
    public static class PlayerGroupReaderHelper
    {
        public static bool IsNewGroup([NotNull]Range range, int row, int column) => GetStringValue(range, row, column).IsNotBlank() && 
                                                                                    GetStringValue(range, row, column+1).IsBlank() &&
                                                                                    GetStringValue(range, row+1, column).IsNotBlank() &&
                                                                                    GetStringValue(range, row+1, column+1).IsBlank();

        [CanBeNull]
        public static PlayersGroup GetGroup([NotNull]Range range, int row, int column) => new PlayersGroup
        {
            Type = GetStringValue(range, row, column),
            Weight = GetStringValue(range, row+1, column)
        };

        public static bool IsPlayer([NotNull]Range range, int row, int column) => GetStringValue(range, row, column).IsNotBlank() &&
                                                                                  GetStringValue(range, row, column + 1).IsNotBlank();

        [CanBeNull]
        public static Player GetPlayer([NotNull]Range range, int row, int column) => new Player
        {
            Initials = GetStringValue(range, row, column),
            Team = GetStringValue(range, row, column+1),
            Mentor = GetStringValue(range, row, column+2)
        };

        [CanBeNull]
        public static string GetStringValue([NotNull] Range range, int row, int column)
        {
            return ((range.Cells[row, column] as Range)
                ?.Value as string)?.Trim();
        }
    }
}
