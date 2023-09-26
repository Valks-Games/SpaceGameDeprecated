global using Godot;
global using GodotUtils;
global using System;
global using System.Collections.Generic;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using System.Threading;
global using System.Text.RegularExpressions;
global using System.Threading.Tasks;

namespace Template;

public partial class Global : Node
{
    [Export] OptionsManager optionsManager;

	public override void _Ready()
	{
        // Gradually fade out all SFX whenever the scene is changed
        SceneManager.SceneChanged += name => 
            AudioManager.Instance.FadeOutSFX();
    }

	public override void _PhysicsProcess(double delta)
	{
		Logger.Update();
	}

    public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			Quit();
		}
	}

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("fullscreen"))
            optionsManager.ToggleFullscreen();
    }

    public void Quit()
	{
        // Handle cleanup here
        optionsManager.SaveOptions();
        optionsManager.SaveHotkeys();

        // This must be here because buttons call Global::Quit()
        GetTree().Quit();
	}
}
