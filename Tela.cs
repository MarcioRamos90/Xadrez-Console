using System;
using tabuleiro;

namespace xadrez_console
{
  public class Tela
  {
    public static void imprimirTabuleiro(Tabuleiro tab)
    {
      for (int i = 0; i < tab.linhas; i++)
      {
        for (int j = 0; j < tab.colunas; j++)
        {
          // SE DER NULO IMPRIME TRAÇO + ESPAÇO
          if (tab.peca(i, j) == null)
          {
            Console.Write("- ");
          }
          else
          {
            // OU ENTÃO IMPRIME PEÇA + ESPAÇO
            Console.Write(tab.peca(i, j) + " ");
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}