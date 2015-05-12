using UnityEngine;
using System.Collections;

// Necessary to make gui skin universal
public class GuiManager : MonoBehaviour 
{
  public GUISkin guiSkin;

  private static GuiManager instance;

  void Awake()
  {
    instance = this;
		DontDestroyOnLoad(this);
  }

  public static GUISkin GetSkin()
  {
    return instance.guiSkin;
  }
}
