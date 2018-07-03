using System;
using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
  public class PatidaDeXadrez
  {
    public Tabuleiro tab { get; private set; }
    public int turno { get; private set; }
    public Cor jogadorAtual { get; private set; }
    public bool termianda { get; private set; }
    private HashSet<Peca> pecas;
    private HashSet<Peca> capturadas;
    public bool xeque { get; set; }

    public PatidaDeXadrez()
    {
      this.tab = new Tabuleiro(8, 8);
      this.turno = 1;
      this.termianda = false;
      this.xeque = false;
      this.pecas = new HashSet<Peca>();
      this.capturadas = new HashSet<Peca>();
      this.jogadorAtual = Cor.Branca;
      colocarPecas();
    }

    public void colocarNovaPeca(char coluna, int linha, Peca peca)
    {
      tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
      pecas.Add(peca);
    }
    public void colocarPecas()
    {

      colocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
      colocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
      colocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
      colocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
      colocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
      colocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));
      colocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
      colocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
      colocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
      colocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
      colocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
      colocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
    }
    private void mudaJogador()
    {
      if (jogadorAtual == Cor.Branca)
      {
        jogadorAtual = Cor.Preta;
      }
      else
      {
        jogadorAtual = Cor.Branca;
      }
    }
    public void realizaJogada(Posicao origem, Posicao destino)
    {
      Peca pecaCapturada = executaMovimento(origem, destino);
      if (estaEmXeque(jogadorAtual))
      {
        desfazMovimento(origem, destino, pecaCapturada);
        throw new TabuleiroException("Você não pode se colocar em xeque!");
      }

      if (estaEmXeque(adversaria(jogadorAtual)))
      {
        xeque = true;
      }
      else
      {
        xeque = false;
      }
      turno++;
      mudaJogador();
    }

    public Cor adversaria(Cor cor)
    {
      if (cor == Cor.Branca)
      {
        return Cor.Preta;
      }
      else
      {
        return Cor.Branca;
      }
    }

    public bool estaEmXeque(Cor cor)
    {
      Peca Rei = rei(cor);
      if (Rei == null)
      {
        throw new TabuleiroException("Não tem rei da cor + " + cor + " no tabuleiro");
      }

      foreach (Peca x in pecasEmJogo(adversaria(cor)))
      {
        bool[,] mat = x.movimentosPossiveis();
        if (mat[Rei.posicao.linha, Rei.posicao.coluna])
        {
          return true;
        }
      }
      return false;
    }

    private Peca rei(Cor cor)
    {
      foreach (Peca x in pecasEmJogo(cor))
      {
        if (x is Rei)
        {
          return x;
        }
      }
      return null;
    }

    public void validarPosicaoDeOrigem(Posicao origem)
    {
      if (tab.peca(origem) == null)
      {
        throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
      }
      if (jogadorAtual != tab.peca(origem).cor)
      {
        throw new TabuleiroException("Peça escolhida não é valida!");
      }
      if (!tab.peca(origem).existeMovimentosPossiveis())
      {
        throw new TabuleiroException("Não há movimentos possiveis para a peça de origem escohida!");
      }
    }

    public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
    {
      if (!tab.peca(origem).podeMoverPara(destino))
      {
        throw new TabuleiroException("Posição de destino inválida!");
      }
    }

    public HashSet<Peca> pecasCapturadas(Cor cor)
    {
      HashSet<Peca> aux = new HashSet<Peca>();
      foreach (Peca x in capturadas)
      {
        if (x.cor == cor)
        {
          aux.Add(x);
        }
      }
      return aux;
    }

    public HashSet<Peca> pecasEmJogo(Cor cor)
    {
      HashSet<Peca> aux = new HashSet<Peca>();
      foreach (Peca x in pecas)
      {
        if (x.cor == cor)
        {
          aux.Add(x);
        }
      }
      aux.ExceptWith(pecasCapturadas(cor));
      return aux;
    }
    public Peca executaMovimento(Posicao origem, Posicao destino)
    {
      Peca p = tab.retirarPeca(origem);
      p.incrementarQteMovimentos();
      Peca pecaCapturada = tab.retirarPeca(destino);
      tab.colocarPeca(p, destino);
      if (pecaCapturada != null)
      {
        capturadas.Add(pecaCapturada);
      }
      return pecaCapturada;
    }

    public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
    {
      Peca p = tab.retirarPeca(destino);
      p.decrementarQteMovimentos();
      if (pecaCapturada != null)
      {
        tab.colocarPeca(pecaCapturada, destino);
        capturadas.Remove(pecaCapturada);
      }
      tab.colocarPeca(p, origem);




    }
  }
}