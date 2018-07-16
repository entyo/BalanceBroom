using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour {
　static Canvas _canvas;
  void Start () {
    _canvas = this.gameObject.GetComponent<Canvas>();
  }

  /// 表示・非表示を設定する
  public static void SetActive(string name, bool isActive) {
    foreach(Transform child in _canvas.transform) {
      if(child.name == name) {
        child.gameObject.SetActive(isActive);
        return;
      }
    }
  }
}
