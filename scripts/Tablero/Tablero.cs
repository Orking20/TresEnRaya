using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Tablero : Control
{
	private Button[,] casillas; 
	private Button reiniciar;
	private bool turnoJugador;
	private Texture2D fichaX;
	private Texture2D fichaO;
	private bool finPartida;

	public override void _Ready()
	{
		this.casillas = new Button[3, 3];
		this.turnoJugador = true;
		this.fichaX = (Texture2D)GD.Load("res://Sprites/Cruz.png");
		this.fichaO = (Texture2D)GD.Load("res://Sprites/Circulo.png");
		this.finPartida = false;

		this.reiniciar = GetNode<Button>("Reiniciar");
		this.reiniciar.Pressed += OnBotonReiniciarPressed;

		this.casillas[0, 0] = GetNode<Button>("ContenedorCasillas/Casilla1");
		this.casillas[0, 1] = GetNode<Button>("ContenedorCasillas/Casilla2");
		this.casillas[0, 2] = GetNode<Button>("ContenedorCasillas/Casilla3");
		this.casillas[1, 0] = GetNode<Button>("ContenedorCasillas/Casilla4");
		this.casillas[1, 1] = GetNode<Button>("ContenedorCasillas/Casilla5");
		this.casillas[1, 2] = GetNode<Button>("ContenedorCasillas/Casilla6");
		this.casillas[2, 0] = GetNode<Button>("ContenedorCasillas/Casilla7");
		this.casillas[2, 1] = GetNode<Button>("ContenedorCasillas/Casilla8");
		this.casillas[2, 2] = GetNode<Button>("ContenedorCasillas/Casilla9");

		foreach (var casilla in this.casillas)
		{
			casilla.CustomMinimumSize = new Vector2(105, 105);
			
			//Callable callable = new Callable(this, MethodName.OnCasillaPresionada);
			//casilla.Connect("pressed", callable.Call(casilla));

			casilla.Pressed += () => OnCasillaPresionada(casilla);
		}
	}

	private void OnCasillaPresionada(Button casilla)
	{
		if (this.turnoJugador)
		{
			casilla.Icon = this.fichaX;
			casilla.Disabled = true;
			this.turnoJugador = false;
			ChequearVictoria(this.fichaX);
			if (!this.finPartida)
			{
				TurnoMaquina();
				ChequearVictoria(this.fichaO);
			}
		}
		else
		{
			casilla.Icon = this.fichaO;
			casilla.Disabled = true;
			this.turnoJugador = true;
			
		}
	}

	private void TurnoMaquina()
	{
		if (!HayEspaciosDisponibles())
			return;
		
		int mejorPuntuacion = int.MinValue;
		int mejorFila = -1;
		int mejorColumna = -1;

		// Probar todas las posibles jugadas
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!casillas[i, j].Disabled)
				{
					// Simular la jugada
					casillas[i, j].Icon = this.fichaO;
					casillas[i, j].Disabled = true;

					// Obtener puntuación de esta jugada
					int puntuacion = Minimax(false);

					// Deshacer la jugada
					casillas[i, j].Icon = null;
					casillas[i, j].Disabled = false;

					// Actualizar la mejor jugada si encontramos una mejor
					if (puntuacion > mejorPuntuacion)
					{
						mejorPuntuacion = puntuacion;
						mejorFila = i;
						mejorColumna = j;
					}
				}
			}
		}

		if (mejorFila != -1 && mejorColumna != -1)
		{
			casillas[mejorFila, mejorColumna].EmitSignal(Button.SignalName.Pressed);
			casillas[mejorFila, mejorColumna]._Pressed();
		}
	}

	private int Minimax(bool esMaximizador)
	{
		// Verificar si hay un ganador o empate
		if (HayGanadorMinimax(fichaO)) return 1;    // La máquina gana
		if (HayGanadorMinimax(fichaX)) return -1;   // El jugador gana
		if (!HayEspaciosDisponibles()) return 0;    // Empate

		if (esMaximizador)
		{
			int mejorPuntuacion = int.MinValue;
			
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (!casillas[i, j].Disabled)
					{
						// Simular jugada
						casillas[i, j].Icon = fichaO;
						casillas[i, j].Disabled = true;

						mejorPuntuacion = Math.Max(mejorPuntuacion, Minimax(false));

						// Deshacer jugada
						casillas[i, j].Icon = null;
						casillas[i, j].Disabled = false;
					}
				}
			}
			return mejorPuntuacion;
		}
		else
		{
			int mejorPuntuacion = int.MaxValue;
			
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (!casillas[i, j].Disabled)
					{
						// Simular jugada
						casillas[i, j].Icon = fichaX;
						casillas[i, j].Disabled = true;

						mejorPuntuacion = Math.Min(mejorPuntuacion, Minimax(true));

						// Deshacer jugada
						casillas[i, j].Icon = null;
						casillas[i, j].Disabled = false;
					}
				}
			}
			return mejorPuntuacion;
		}
	}

	private bool HayGanadorMinimax(Texture2D ficha)
	{
		// Revisar filas y columnas
		for (int i = 0; i < 3; i++)
		{
			if ((casillas[i, 0].Icon == ficha && casillas[i, 1].Icon == ficha && casillas[i, 2].Icon == ficha) ||
				(casillas[0, i].Icon == ficha && casillas[1, i].Icon == ficha && casillas[2, i].Icon == ficha))
			{
				return true;
			}
		}

		// Revisar diagonales
		if ((casillas[0, 0].Icon == ficha && casillas[1, 1].Icon == ficha && casillas[2, 2].Icon == ficha) ||
			(casillas[0, 2].Icon == ficha && casillas[1, 1].Icon == ficha && casillas[2, 0].Icon == ficha))
		{
			return true;
		}

		return false;
	}

	private void ChequearVictoria(Texture2D ficha)
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.casillas[i, 0].Icon == ficha && this.casillas[i, 1].Icon == ficha && this.casillas[i, 2].Icon == ficha ||
			this.casillas[0, i].Icon == ficha && this.casillas[1, i].Icon == ficha && this.casillas[2, i].Icon == ficha)
			{
				AnunciarVictoria(ficha);
				this.finPartida = true;
				return;
			}
		}

		if (this.casillas[0, 0].Icon == ficha && this.casillas[1, 1].Icon == ficha && this.casillas[2, 2].Icon == ficha ||
		this.casillas[0, 2].Icon == ficha && this.casillas[1, 1].Icon == ficha && this.casillas[2, 0].Icon == ficha)
		{
			AnunciarVictoria(ficha);
			this.finPartida = true;
			return;
		}
	}

	private void AnunciarVictoria(Texture2D ficha)
	{
		foreach (var casilla in this.casillas)
		{
			casilla.Disabled = true;
		}

		Label ganador = GetNode<Label>("Ganador");

		if (ficha == this.fichaX)
		{
			ganador.Text = "Victoria!";
			ganador.Modulate = new Color(0, 255, 0);
		}
		else
		{
			ganador.Text = "Derrota";
			ganador.Modulate = new Color(255, 0, 0);
		}

		ganador.AddThemeFontSizeOverride("font_size", 32);
		ganador.HorizontalAlignment = HorizontalAlignment.Center;
		ganador.VerticalAlignment = VerticalAlignment.Bottom;
	}

	private bool HayEspaciosDisponibles()
	{
		foreach (var casilla in this.casillas)
		{
			if (!casilla.Disabled)
				return true;
		}
		return false;
	}

	private void OnBotonReiniciarPressed()
	{
		GetTree().ReloadCurrentScene();
	}
}
