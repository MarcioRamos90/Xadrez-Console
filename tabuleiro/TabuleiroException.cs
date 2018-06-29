using System;

namespace tabuleiro
{
  [System.Serializable]
  public class TabuleiroException : System.Exception
  {
    public TabuleiroException(string message) : base(message) { }
  }
}