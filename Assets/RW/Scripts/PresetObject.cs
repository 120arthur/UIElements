/*
* Copyright (c) 2020 Razeware LLC
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* Notwithstanding the foregoing, you may not use, copy, modify, merge, publish,
* distribute, sublicense, create a derivative work, and/or sell copies of the
* Software in any work that is designed, intended, or marketed for pedagogical or
* instructional purposes related to programming, coding, application development,
* or information technology.  Permission for such use, copying, modification,
* merger, publication, distribution, sublicensing, creation of derivative works,
* or sale is expressly withheld.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PresetObject : MonoBehaviour
{

    public int appliedPreset = 0;

    [Header("Preset List")]
    public List<Preset> presets;

    [Header("References")]
    public Animator animator;

    [Header("Currently Editing")]
    public Preset currentlyEditing;

    //This method can be called to change this GameObject's properties based on a Preset
    public void ApplyPreset(Preset preset)
    {
        //Changing the name of the object
        transform.name = preset.objectName;

        //Changing the size of the object
        transform.localScale = preset.size;

        //Changing the rotation of the object
        transform.eulerAngles = preset.rotation;

        //Setting the color of the object
        transform.GetComponent<MeshRenderer>().material.color = preset.color;

        //Changing the speed and state of the animation
        if (preset.isAnimating)
        {
            animator.speed = preset.animationSpeed;
        }
        else
        {
            animator.speed = 0f;
        }

    }

    public void Update()
    {
    	if (presets.Count <= 0)
        {
            return;
        }

        if (presets[appliedPreset] != null)
        {
            ApplyPreset(presets[appliedPreset]);
        }

    }

}

