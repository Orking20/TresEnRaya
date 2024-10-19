using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Tablero : Control
{
	private Button[,] casillas; 
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

	public override void _Process(double delta)
	{
	}

	private void OnCasillaPresionada(Button casilla)
	{
		if (this.turnoJugador)
		{
			casilla.Icon = this.fichaX;
			casilla.Disabled = true;
			this.turnoJugador = false;
			ChequearVictoria();
			if (!this.finPartida)
			{
				TurnoMaquina();
			}
		}
		else
		{
			casilla.Icon = this.fichaO;
			casilla.Disabled = true;
			this.turnoJugador = true;
			ChequearVictoria();
		}
	}

	private void TurnoMaquina()
	{
		bool fichaMarcada = false;
		bool hayEspacio = false;
		var fila = new Random().Next(0, 3);
		var columna = new Random().Next(0, 3);

		foreach (var casilla in this.casillas)
		{
			if (!casilla.Disabled)
			{
				hayEspacio = true;
			}
		}

		while (!fichaMarcada && hayEspacio)
		{
			if (this.casillas[fila, columna].Disabled == false)
			{
				this.casillas[fila, columna].EmitSignal(Button.SignalName.Pressed);
				this.casillas[fila, columna]._Pressed();
				fichaMarcada = true;
			}
			else
			{
				fila = new Random().Next(0, 3);
				columna = new Random().Next(0, 3);
			}
		}
	}

	private void ChequearVictoria()
	{
		for (int i = 0; i < 3; i++)
		{
			if (this.casillas[i, 0].Icon == this.fichaX && this.casillas[i, 1].Icon == this.fichaX && this.casillas[i, 2].Icon == this.fichaX ||
			this.casillas[0, i].Icon == this.fichaX && this.casillas[1, i].Icon == this.fichaX && this.casillas[2, i].Icon == this.fichaX)
			{
				AnunciarVictoria();
				this.finPartida = true;
				return;
			}
		}

		if (this.casillas[0, 0].Icon == this.fichaX && this.casillas[1, 1].Icon == this.fichaX && this.casillas[2, 2].Icon == this.fichaX ||
		this.casillas[0, 2].Icon == this.fichaX && this.casillas[1, 1].Icon == this.fichaX && this.casillas[2, 0].Icon == this.fichaX)
		{
			AnunciarVictoria();
			this.finPartida = true;
			return;
		}

		for (int i = 0; i < 3; i++)
		{
			if (this.casillas[i, 0].Icon == this.fichaO && this.casillas[i, 1].Icon == this.fichaO && this.casillas[i, 2].Icon == this.fichaO ||
			this.casillas[0, i].Icon == this.fichaO && this.casillas[1, i].Icon == this.fichaO && this.casillas[2, i].Icon == this.fichaO)
			{
				AnunciarDerrota();
				this.finPartida = true;
				return;
			}
		}

		if (this.casillas[0, 0].Icon == this.fichaO && this.casillas[1, 1].Icon == this.fichaO && this.casillas[2, 2].Icon == this.fichaO ||
		this.casillas[0, 2].Icon == this.fichaO && this.casillas[1, 1].Icon == this.fichaO && this.casillas[2, 0].Icon == this.fichaO)
		{
			AnunciarDerrota();
			this.finPartida = true;
			return;
		}
	}

	private void AnunciarVictoria()
	{
		foreach (var casilla in this.casillas)
		{
			casilla.Disabled = true;
		}

		Label ganador = GetNode<Label>("Ganador");
		ganador.Text = "Victoria!";
		ganador.AddThemeFontSizeOverride("font_size", 32);
		ganador.HorizontalAlignment = HorizontalAlignment.Center;
		ganador.VerticalAlignment = VerticalAlignment.Bottom;
		ganador.Modulate = new Color(0, 255, 0);
	}

	private void AnunciarDerrota()
	{
		foreach (var casilla in this.casillas)
		{
			casilla.Disabled = true;
		}

		Label ganador = GetNode<Label>("Ganador");
		ganador.Text = "Derrota";
		ganador.AddThemeFontSizeOverride("font_size", 32);
		ganador.HorizontalAlignment = HorizontalAlignment.Center;
		ganador.VerticalAlignment = VerticalAlignment.Bottom;
		ganador.Modulate = new Color(255, 0, 0);
	}
}
