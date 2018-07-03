using System;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        PatidaDeXadrez partida = new PatidaDeXadrez();

        while (!partida.termianda)
        {
          try
          {
            Console.Clear();
            Tela.imprimriPartida(partida);

            Console.WriteLine();
            Console.Write("Origem: ");
            Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
            partida.validarPosicaoDeOrigem(origem);

            bool[,] possicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();

            Console.Clear();
            Tela.imprimirTabuleiro(partida.tab, possicoesPossiveis);

            Console.WriteLine();
            Console.Write("Destino: ");
            Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
            partida.validarPosicaoDeDestino(origem, destino);

            partida.realizaJogada(origem, destino);
          }
          catch (TabuleiroException e)
          {
            Console.WriteLine(e.Message);
            Console.ReadLine();
          }
        }



      }
      catch (TabuleiroException e)
      {
        Console.WriteLine(e.Message);
      }
    }


  }
}
