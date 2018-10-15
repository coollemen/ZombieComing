using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace GameFramework
{
    public class GameEventArgs
    {
        public int Type { get; set; }

        public System.Object[] Params { get; set; }

        public System.Object Sender { get; set; }

        public override string ToString()
        {
            string arg = null;
            if (Params != null)
            {
                for (int i = 0; i < Params.Length; i++)
                {
                    if ((Params.Length > 1 && Params.Length - 1 == i) || Params.Length == 1)
                    {
                        arg += Params[i];
                    }
                    else
                    {
                        arg += Params[i] + " , ";
                    }
                }
            }

            return Type + " [ " + ((Sender == null) ? "null" : Sender.ToString()) + " ] " + " [ " +
                   ((arg == null) ? "null" : arg.ToString()) + " ] ";
        }

        public GameEventArgs Clone()
        {
            return new GameEventArgs(Type, Params, Sender);
        }

        public GameEventArgs()
        {
            
        }
        public GameEventArgs(int type)
        {
            Type = type;
        }

        public GameEventArgs(int type, params System.Object[] param)
        {
            Type = type;
            Params = param;
        }

        public GameEventArgs(int type, System.Object sender, params System.Object[] param)
        {
            Type = type;
            Params = param;
            Sender = sender;
        }
    }
}
