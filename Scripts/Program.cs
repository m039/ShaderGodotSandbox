using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Program : Node
{
    PopupMenu _shadersPopupMenu;

    ShaderMaterial _shaderMaterial;

    string _firstShader;

    string _selectedShader;

    FileSystemWatcher _watcher;

    public override void _Ready()
    {
        _shadersPopupMenu = GetNode<PopupMenu>("VBoxContainer/MenuBar/Shaders");
        _shadersPopupMenu.Clear();
        _shaderMaterial = (ShaderMaterial)GetNode<Control>("VBoxContainer/Panel").Material;	
        _watcher = new FileSystemWatcher("Shaders");
        _watcher.NotifyFilter = NotifyFilters.Attributes | 
            NotifyFilters.LastWrite;

        _watcher.Changed += OnChanged;
        _watcher.Filter = "*.gdshader";
        _watcher.EnableRaisingEvents = true;

        PopulatePopupMenu();
        SelectShader(_firstShader);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        SelectShader(_selectedShader);
    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == NotificationPredelete) {
            _watcher?.Dispose();
            _watcher = null;
        }
    }

    void PopulatePopupMenu()
    {
        var names = new List<string>();

        foreach (var file in Directory.GetFiles("./Shaders", "*.gdshader"))
        {
            var fileName = Path.GetFileName(file);
            var trimmedName = fileName.Substring(0, fileName.Find(".gdshader"));

            names.Add(trimmedName);
        }

        names.Sort();

        foreach (var name in names)
        {
            _shadersPopupMenu.AddItem(name);

            if (_firstShader == null) {
                _firstShader = name;
            }
        }
    }

    private void OnShadersIdPressed(long id)
    {
        SelectShader(_shadersPopupMenu.GetItemText(_shadersPopupMenu.GetItemIndex((int)id)));
    }

    void SelectShader(string shaderName)
    {
        _shaderMaterial.Shader.Code = File.ReadAllText(Path.Combine("Shaders", shaderName + ".gdshader"));
        _selectedShader = shaderName;
    }
}
