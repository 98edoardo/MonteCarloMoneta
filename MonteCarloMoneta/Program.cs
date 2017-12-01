using System;

namespace MonteCarloMoneta {

	public class Punto {

		public double X;
		public double Y;

		public Punto(double x, double y) {
			X = x;
			Y = y;
		}

		public double Distanza(Punto p) {

			return Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));
		}
	}

	public class Cerchio {

		public Punto Centro;

		public double Raggio {
			get;
			private set;
		}

		public Cerchio(Punto centro, double raggio) {
			Centro = centro;
			Raggio = raggio;
		}

		public bool Dentro(Punto p) {

			if (Centro.Distanza(p) <= 1)
				return true;

			return false;
		}
	}

	public class Rettangolo {

		public Punto SxBasso;
		public Punto SxAlto;
		public Punto DxBasso;
		public Punto DxAlto;

		public double Base {
			get;
			private set;
		}

		public double Altezza {
			get;
			private set;
		}

		public Rettangolo(double baseRettangolo, double altezzaRettangolo) {

			SxAlto = new Punto(-1 * baseRettangolo / 2, altezzaRettangolo / 2);
			SxBasso = new Punto(-1 * baseRettangolo / 2, -1 * altezzaRettangolo / 2);
			DxAlto = new Punto(baseRettangolo / 2, altezzaRettangolo / 2);
			DxBasso = new Punto(baseRettangolo / 2, -1 * altezzaRettangolo / 2);
		}

		public bool Dentro(Punto p) {

			if (SxAlto.X > p.X || SxAlto.Y < p.Y)
				return false;

			if (SxBasso.X > p.X || SxBasso.Y > p.Y)
				return false;

			if (DxAlto.X < p.X || DxAlto.Y < p.Y)
				return false;

			if (DxBasso.X < p.X || DxBasso.Y > p.Y)
				return false;

			return true;
		}
	}

	class MonteCarlo {

		private Cerchio _moneta;
		private Rettangolo _piastrella;
		private Rettangolo _confineInterno;
		private Random _casuale = new Random();

		public uint NumeroTentativi {
			get;
			private set;
		}

		public uint ASegno {
			get;
			private set;
		}

		public MonteCarlo(Cerchio moneta, Rettangolo piastrella) {

			_piastrella = _confineInterno = piastrella;
			_moneta = moneta;

			double tolleranza = _moneta.Raggio / 2;

			_confineInterno.SxAlto.X += tolleranza;
			_confineInterno.SxAlto.Y -= tolleranza;

			_confineInterno.SxBasso.X += tolleranza;
			_confineInterno.SxBasso.Y += tolleranza;

			_confineInterno.DxAlto.X -= tolleranza;
			_confineInterno.DxAlto.Y -= tolleranza;

			_confineInterno.DxBasso.X -= tolleranza;
			_confineInterno.DxBasso.Y += tolleranza;
		}

		public void Tenta() {

			Punto p = new Punto(GetRandomNumber(-1 * _piastrella.Base / 2, _piastrella.Base / 2), GetRandomNumber(-1 * _piastrella.Altezza / 2, _piastrella.Altezza / 2));

			NumeroTentativi++;

			if (_confineInterno.Dentro(p) == true)
				ASegno++;
		}

		private double GetRandomNumber(double min, double max) {
			double a = _casuale.NextDouble() * (max - min) + min;
			return a;
		}
	}

	class MainClass {
		
		public static void Main(string[] args) {
			MonteCarlo m = new MonteCarlo(new Cerchio(new Punto(0, 0), 1), new Rettangolo(5, 5));

			for (int i = 0; i < 100000; i++)
				m.Tenta();

			Console.WriteLine("{0}", m.ASegno / m.NumeroTentativi * 100);

			Console.ReadKey();

		}
	}
}
