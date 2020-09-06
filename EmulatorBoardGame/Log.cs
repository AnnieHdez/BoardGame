using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmulatorBoardGame
{
    public enum LogKind
    {
        StartTournament,
        EndTournament,
        StartMatch,
        EndMatch,
        StartGame,
        EndGame,
        Play
    }
    public class LogArg : EventArgs
    {
        public string Message { get; private set; }

        public LogArg(string message)
        {
            Message = message;
        }
    }
    public static class Log
    {
        public static event EventHandler<LogArg> StartTournament;
        public static event EventHandler<LogArg> EndTournament;
        public static event EventHandler<LogArg> StartMatch;
        public static event EventHandler<LogArg> EndMatch;
        public static event EventHandler<LogArg> StartGame;
        public static event EventHandler<LogArg> EndGame;
        public static event EventHandler<LogArg> Play;

        public static void Write(LogKind logKind, string message)
        {
        
                if(logKind== LogKind.StartTournament)
                {
                    if (StartTournament != null)
                        StartTournament(null, new LogArg(message));
                }
                if(logKind== LogKind.EndTournament)
                {
                    if (EndTournament != null)
                        EndTournament(null, new LogArg(message));
                }
                if(logKind== LogKind.StartMatch)
                {
                    if (StartMatch != null)
                        StartMatch(null, new LogArg(message));
                }
                if(logKind== LogKind.EndMatch)
                {
                    if (EndMatch != null)
                        EndMatch(null, new LogArg(message));
                }
                   
                if(logKind==LogKind.StartGame)
                {
                    if (StartGame != null)
                        StartGame(null, new LogArg(message));
                }
               if(logKind== LogKind.EndGame)
               {
                    if (EndGame != null)
                        EndGame(null, new LogArg(message));
               }
                if(logKind== LogKind.Play)
                {
                    if (Play != null)
                        Play(null, new LogArg(message));
                }
                
            }
        }
    }

