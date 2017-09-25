using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace WindowsFormsApplication1 {
	public class TTT {
		public static int N = 3;
		public int turno = 0;
		public SortedSet<short>[] winners = new SortedSet<short>[2*N + 2];
		public SortedSet<short> playerMoves = new SortedSet<short>();
		public SortedSet<short> comMoves = new SortedSet<short>();
		public enum GameState { Player = 0, COM = 1, Win = 2, Lose = 3, Draw = 4 };
		public GameState GS;


		public TTT()   {

			//////////Inicializacion de los sets////////////////
			for(int k = 0; k < 2* N + 2;k++) {
				winners[k] = new SortedSet<short>();
			}

			///////////////////horitzontales////////////////////
			int itSet = 0;
			int aux = 0;
			for(int i = 0;i < N*N; i++){
				winners[itSet].Add((short)i);
				if(aux == N-1) {
					itSet++;
					aux = 0;
				}
				else aux++;
			}
		
			//////////////verticales////////////////////////
			for(int i = N; i < 2*N; i++) {
				for(int j = 0;j < N;j++) {
					int aux2 = (i-N) + (N * j);
					winners[i].Add((short)aux2);
				}
			}
			/////////////diagonal principal///////////////////////
			for(int i = 0; i < N*N;i+=N+1) {
				winners[2 * N].Add((short)i);
			}


			////////////////diagonal secundaria////////////////
			for(int i = N-1;i <= N*(N-1); i += N - 1) {
				winners[2 * N + 1].Add((short)i);
			}
		}

		public void Refresh(int i, int turn) {
			if( turn == 0) {
				playerMoves.Add((short)i);
			}

			if(turn == 1) {
				comMoves.Add((short)i);
			}
		}

		public void CheckWin(int turn) {
			/*if(turn == 0) {
				for(int i = 0; i < winners.Count();i++) {
					if(winners[i].IsSubsetOf(playerMoves) && playerMoves.Count > 2) {
						GS = GameState.Win;
					}
				}
			}
			
			if(turn == 1) {
				for(int i = 0;i < winners.Count();i++) {
					if(winners[i].IsSubsetOf(comMoves) && comMoves.Count > 2) {
						GS = GameState.Lose;
					}
					
				}
			}*/
			int count = 0;
			bool flag = true;
			for(int i = 0;i < winners.Count();i++) {
				count = 0;
				flag = true;
				for(int j = 0; j < winners[i].Count(); j++) {
					if(turn == 1 && flag == true) {
						if(comMoves.Contains(winners[i].ElementAt(j))) {
							count++;
						}
						else flag = false;
					}
					if(turn == 0 && flag == true) {
						if(playerMoves.Contains(winners[i].ElementAt(j))) {
							count++;
						}
						else flag = false;
					}
				}
				if(count == 3) {
					if(turn == 1) GS = GameState.Lose;
					if(turn == 0) GS = GameState.Win;
				}

			}
		}

		public int comMove(int turn, int turnCount) {
			Random randomIndex = new Random();
			int[] diag = new int[4]{0,2,6,8};
			int[] sides = new int[4] {1,3,5,7};
			int center = 4;
			turno = turn;
			int result = -1;

			if(turn == 1) {
				switch(turnCount) {
					case 0:
					result = diag[randomIndex.Next(0,4)];
					break;

					case 2:
					if(playerMoves.Contains(4)) {
						result = 1;
					}

					if(playerMoves.ElementAt(0) % 2 == 1) { //jugador en arista
						result = center;
					}

					else {//jugador en diagonal
						result = cubrir();
						if(result == -1) result = diag[randomIndex.Next(4)];
					}
					break;


					case 1:
					if(!playerMoves.Contains(4)) return 4;
					else result = diag[randomIndex.Next(0,4)];
					break;

					
					case 3:
						if((playerMoves.Contains(0) && playerMoves.Contains(8)) || (playerMoves.Contains(2) && playerMoves.Contains(4))) {
							result = sides[randomIndex.Next(0,4)]; //jugada diagonal opuesta
						}
					else {
						result = cubrir();
						if(playerMoves.Contains(0) && result == -1) {
							return sides[randomIndex.Next(2,4)];
						}
					}
					break;

					default:
					result = ganar();
					if(result == -1) {
						result = cubrir();
						if(result == -1) {
							result = randomMove();
						}
					}


					break;
				}
			}
			if(result != -1) return result;
			else return randomMove();
		}

		public int cubrir() {
			int result = -1;
			int i = 0;
			bool flag = false;
			while(i < winners.Count() && flag == false){
				SortedSet<short> aux = new SortedSet<short>();
				SortedSet<short> aux2 = new SortedSet<short>();
				SortedSet<short> aux3 = new SortedSet<short>();
				aux.UnionWith(playerMoves);
				aux2.UnionWith(comMoves);
				aux3.UnionWith(winners[i]);
				aux.IntersectWith((IEnumerable<short>)aux3);
				aux2.IntersectWith((IEnumerable<short>)aux3);
				if(aux.Count() == 2 && aux2.Count() == 0) {
					result = moveToWin(aux3,0);
					flag = true;
				}
				i++;
			}
			return result;
		}

		public int ganar() {
			int result = -1;
			int i = 0;
			bool flag = false;
			while(i < winners.Count() && flag == false) {
				SortedSet<short> aux = new SortedSet<short>();
				SortedSet<short> aux2 = new SortedSet<short>();
				SortedSet<short> aux3 = new SortedSet<short>();
				aux.UnionWith(playerMoves);
				aux2.UnionWith(comMoves);
				aux3.UnionWith(winners[i]);
				aux.IntersectWith((IEnumerable<short>)aux3);
				aux2.IntersectWith((IEnumerable<short>)aux3);
				if(aux.Count() == 0 && aux2.Count() == 2) {
					result = moveToWin(aux3,1);
					flag = true;
				}
				i++;
			}
			result = checkMove(result);
			return result;
		}

		public int randomMove() {
			Random random = new Random();
			int result = -1;
			bool flag = false;
			while(flag == false) {
				result = random.Next(0,9);
				result = checkMove(result);
				if(result != -1) flag = true;
			}
			return result;
		}

		public int checkMove(int winner) {
			int result = -1;
			int i = 0;
			bool flag = false;
			/*while(i < winners[winner].Count() && flag == false) {
				if(!playerMoves.Contains(winners[winner].ElementAt(i)) && !comMoves.Contains(winners[winner].ElementAt(i))) {
					result = winners[winner].ElementAt(i);
					flag = true;
				}
				i++;
			}*/
			if(!playerMoves.Contains((short)winner) && !comMoves.Contains((short)winner)){
				result = winner;
				flag = true;
			}
			return result;
		}
		
		public int attack() {
			int result = -1;
			int i = 0;
			bool flag = false;
			while(i < winners.Count() && flag == false) {
				SortedSet<short> aux = new SortedSet<short>();
				SortedSet<short> aux2 = new SortedSet<short>();
				SortedSet<short> aux3 = new SortedSet<short>();
				aux.UnionWith(playerMoves);
				aux2.UnionWith(comMoves);
				aux3.UnionWith(winners[i]);
				aux.IntersectWith((IEnumerable<short>)aux3);
				aux2.IntersectWith((IEnumerable<short>)aux3);
				if(aux.Count() == 0 && aux2.Count() >= 1) {
					result = moveToWin(aux3,1);
					flag = true;
				}
				i++;
			}

			if(result != -1) return result;
			else return randomMove();
		}

		public int moveToWin(SortedSet<short> winner, int turno) {
			int result = -1;
			SortedSet<short> auxSet = new SortedSet<short>();
			SortedSet<short> auxSet2 = new SortedSet<short>();
			if (turno == 1)auxSet = comMoves;
			else auxSet = playerMoves;
			auxSet2 = winner;
			auxSet.IntersectWith((IEnumerable<short>)auxSet2);
			for(int i = 0;i < auxSet2.Count;i++) {
				auxSet2.Remove(auxSet.ElementAt(i));
			}
			result =  auxSet2.ElementAt(0);
			return result;

		}
	}
}