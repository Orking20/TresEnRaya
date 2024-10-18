using Godot;
using System;

public partial class MenuPrincipal : Control
{
	private Button btnSalir;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.btnSalir = GetNode<Button>("BotonesCentrados/ContenedorBotones/BtnSalir");
		
		//btnSalir.Connect("pressed", new Callable(this, nameof(OnBotonSalirPressed)));
		this.btnSalir.Pressed += OnBotonSalirPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBotonSalirPressed()
	{
		GetTree().Quit();
	}
}
