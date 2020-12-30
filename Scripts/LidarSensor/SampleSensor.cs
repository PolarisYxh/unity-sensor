/*
* MIT License
*
* Copyright (c) 2017 Philip Tibom, Jonathan Jansson, Rickard Laurenius,
* Tobias Alldén, Martin Chemander, Sherry Davar
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// Author: Philip Tibom
/// Simulates the lidar sensor by using ray casting.
/// </summary>
public class SampleSensor : MonoBehaviour {

    private List<Ray> lasers;
    public int numberX;
    public int numberY;

    private bool isPlaying = false;
    private RaycastHit hit;

    // Use this for initialization
    private void Start()
    {
        InitiateLasers();
    }

    private void InitiateLasers()
    {
        lasers = new List<Ray>();
        Ray ray = new Ray();
        Vector3 direction = new Vector3(0, -1, 0);
        StreamWriter streamWriter = new StreamWriter("ps1.txt", true);
        for (int i = 0; i < numberX; i++)//add vertical lasers
        {
            float x = (i-numberX/2)*0.1f;
            for (int j = 0; j < numberY; j++)//add vertical lasers
            {
                float y = (j - numberY/2)*0.1f;
                ray.origin = new Vector3(x, 3, y);
                ray.direction = direction;
                bool isHit = Physics.Raycast(ray, out hit, 7);
                if (isHit)
                {
                    Vector3 pos = hit.point;
                    //在碰撞位置处的UV纹理坐标。
                    Vector2 pixelUV = hit.textureCoord;

                    Material m_material = hit.transform.GetComponent<MeshRenderer>().sharedMaterial;
                    Texture2D m_texture = m_material.mainTexture as Texture2D;
                    if(m_texture)
                    {
                    //以像素为单位的纹理宽度
                    pixelUV.x *= m_texture.width;
                    pixelUV.y *= m_texture.height;

                    //贴图UV坐标以右上角为原点
                    Color c = m_texture.GetPixel((int)pixelUV.x,(int)pixelUV.y);

                    streamWriter.WriteLine(pos.x+" "+pos.y+" "+pos.z+" "+c.r+" "+c.g+" "+c.b);
                    //刷新缓存
                    streamWriter.Flush();
                    }
                }
            }
        }
        //关闭流
        streamWriter.Close();
        isPlaying = true;
    }

    public void PauseSensor(bool simulationModeOn)
    {
    }

    private void FixedUpdate()
    {
    }
}
