using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using WMPLib;

namespace WindowsFormsApplication1 {
	public partial class Form1:Form {
		public Form1() {
			InitializeComponent();
		}
		public const int N = 3;
		public TTT ttt = new TTT();
		public int  turno = 0;
		public int numTurns = 0;
		public Button[] V_;
		public WindowsMediaPlayer wplayer = new WindowsMediaPlayer();
		public WindowsMediaPlayer wplayerWin = new WindowsMediaPlayer();
		public WindowsMediaPlayer wplayerWin2 = new WindowsMediaPlayer();
		public WindowsMediaPlayer wplayerLose = new WindowsMediaPlayer();
		public WindowsMediaPlayer wplayerDraw = new WindowsMediaPlayer();
		public DialogResult option = MessageBox.Show("¿Desea jugar contra la máquina?"," ",MessageBoxButtons.YesNo);


		private void button1_Click(object sender,EventArgs e) {
			Button Baux = (Button)sender;
			if(Baux.Image == null) {
				if(option == DialogResult.Yes) {
					int index = Convert.ToInt32(Baux.Name.Substring(6));
					int comMove = 0;
					ttt.Refresh(index,turno);
					paint((Button)sender,turno);
					if(numTurns >= 5 && numTurns <= 9) ttt.CheckWin(turno);
					turno = 1;
					if(numTurns < 8) comMove = ttt.comMove(turno,numTurns);
					ttt.Refresh(comMove,turno);
					paint(V_[comMove],turno);
					if(numTurns >= 5 && numTurns <= 8) ttt.CheckWin(turno);
					turno = 0;
				}

				if(option == DialogResult.No) {
					int index = Convert.ToInt32(Baux.Name.Substring(6));
					ttt.Refresh(index,turno);
					paint((Button)sender,turno);
					if(numTurns >= 5 && numTurns <= 8) ttt.CheckWin(turno);
					if(turno == 0) {
						turno = 1;
					}
					else turno = 0;
				}

				if(ttt.GS == TTT.GameState.Win) {
					wplayerWin.controls.play();
					if(option == DialogResult.Yes) MessageBox.Show("GANASTE");
					MessageBox.Show("GANA EQUIS");
				}
				if(ttt.GS == TTT.GameState.Lose) {

					if(option == DialogResult.Yes) {
						MessageBox.Show("PERDISTE");
						wplayerLose.controls.play();
					}
					else {
						wplayerWin2.controls.play();
						MessageBox.Show("GANA CIRCULO");
					}
				}

				if(numTurns == 9) {
					ttt.GS = TTT.GameState.Draw;
					wplayerDraw.controls.play();
					MessageBox.Show("EMPATE");
				}
			}
		}

		private void erase(Button boton) {
			boton.Image = null;
		}



		private void paint(Button botonaso, int turnaso) {
			if(turno == 0) {
				if(botonaso.Image == null) {
					botonaso.Image = Image.FromFile(@"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\equis.png");
					wplayer.controls.play();
					numTurns++;
				}
			}
			else {
				if(botonaso.Image == null) {
					botonaso.Image = Image.FromFile(@"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\circulo2.png");
					wplayer.controls.play();
					numTurns++;
				}
			}

		}

		private void Form1_Load(object sender,EventArgs e) {
			wplayerWin.controls.pause();
			wplayerWin2.controls.pause();
			wplayerLose.controls.pause();
			wplayerDraw.controls.pause();
			wplayer.controls.pause();

			wplayer.URL = @"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\hitmarker.mp3";
			wplayerWin.URL = @"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\oh-baby-a-triple.mp3";
			wplayerWin2.URL = @"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\and-his-name-is-john-cena-1.mp3";
			wplayerLose.URL = @"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\yanp.mp3";
			wplayerDraw.URL = @"c:\users\carlos\documents\visual studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Resources\movie_1.mp3";
			
			V_ = new Button[N*N];
			int k = N*N-1;
			for(int i = 0;i < N*N;i++) { 
					V_[i] = (Button)(panel1.Controls[k]);
					k--;
				}
			
			}

		private void button9_Click(object sender,EventArgs e) {
			Reset();
		}


		private void Reset() {
			ttt = new TTT();
			for(int i = 0;i < V_.Count();i++) {
				erase(V_[i]);
			}
			turno = 0;
			numTurns = 0;
		}
	}
}
