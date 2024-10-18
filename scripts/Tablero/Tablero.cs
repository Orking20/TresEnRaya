using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Tablero : Control
{
	private Button[,] casillas; 
	private bool turnoJugador;
	private Texture2D fichaX;
	private Texture2D fichaO;

	public override void _Ready()
	{
		this.casillas = new Button[3, 3];
		this.turnoJugador = true;
		this.fichaX = (Texture2D)GD.Load("res://Sprites/Cruz.png");
		this.fichaO = (Texture2D)GD.Load("res://Sprites/Circulo.png");

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
		}
	}
}
