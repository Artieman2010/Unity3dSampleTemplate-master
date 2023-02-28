using JetBrains.Annotations;
using Nethereum.ABI;

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

using System.Drawing.Imaging;
using FontStyle = System.Drawing.FontStyle;
using Color = System.Drawing.Color;
using Mirror;
using Graphics = System.Drawing.Graphics;
using Font = System.Drawing.Font;
using UnityEngine.UIElements;

public class CaptchaScript : NetworkBehaviour
{
   
   public List<CaptchaCtor> bytes = new List<CaptchaCtor>();
    public List<ViewsandIndexa> sentImagesandViews = new List<ViewsandIndexa>();
  
    public byte[] GenerateCaptchaImage(string text)
    {
        var font = new Font("Arial", 20, FontStyle.Bold);
        var size = new Size(200, 50);
        var bgColor = Color.White;
        var textColor = Color.Black;
        var rand = new System.Random();
        var textBrush = new SolidBrush(textColor);
        var image = new Bitmap(size.Width, size.Height);

        using (var graphics = Graphics.FromImage(image))
        {
            // set background color
            graphics.Clear(bgColor);
            // add random noise
            for (var i = 0; i < 100; i++)
            {
                var x = rand.Next(image.Width);
                var y = rand.Next(image.Height);
                var color = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                image.SetPixel(x, y, color);
            }
            // add random lines
            for (var i = 0; i < 5; i++)
            {
                var x1 = rand.Next(image.Width);
                var y1 = rand.Next(image.Height);
                var x2 = rand.Next(image.Width);
                var y2 = rand.Next(image.Height);
                var pen = new Pen(textColor, 2);
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            // center the text
            var textSize = graphics.MeasureString(text, font);
            var xd = (size.Width - textSize.Width) / 2;
            var yd = (size.Height - textSize.Height) / 2;
            // warp the text
            var path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new PointF(xd, yd), StringFormat.GenericDefault);
            var matrix = new Matrix();
            matrix.Shear(rand.Next(-1, 1) / 10.0f, rand.Next(-1, 1) / 10.0f);
            path.Transform(matrix);
            graphics.SetClip(new RectangleF(0, 0, size.Width, size.Height));
            // add warped text
            graphics.FillPath(textBrush, path);
            // create a memory stream and save the image
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                var imageBytes = ms.ToArray();
                return imageBytes;
            }
        }
    }





    // Start is called before the first frame update
    void Start()
    {
      
        CmdgetTheCaptcha(NetworkClient.connection.connectionId);
        
    }
   

    // Update is called once per frame
    void Update()
    {
       
    }
    public  UnityEngine.UI.Image image;

    public void LoadImage(byte[] imageData)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageData);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
    [Command(requiresAuthority = false)]
    public void CmdgetTheCaptcha(int player) //rpcsecure
    {
       
       

            
          
                // Perform action
             
                var random = new System.Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var randomString = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());


                var haha = GenerateCaptchaImage(randomString);



                bytes.Add(new CaptchaCtor(haha, randomString));
                var l = sentImagesandViews.Find(x => x.connectionID == player);
                if (l == null)
                {
                    TargetgivetheCaptcha(NetworkServer.connections[player], haha);
                    //givecpathca and send haha var as data
                    sentImagesandViews.Add(new ViewsandIndexa(player, bytes.Count - 1));

                }
                else
                {
                    sentImagesandViews.Remove(l);
                    TargetgivetheCaptcha(NetworkServer.connections[player], haha);
                    //givecpathca and send haha var as data
                    sentImagesandViews.Add(new ViewsandIndexa(player, bytes.Count - 1));

                }
            
          
           

        //remove from list once they submit
    }
   //could it be do removing netID
    [TargetRpc]
    void TargetgivetheCaptcha(NetworkConnection conn,  byte[] imageData)
    {
        //change the imageobject into this data
        LoadImage(imageData);

        
    }
    
    [Command(requiresAuthority = false)]
    void CheckAnswer(string playerAnswer, int player) //Rpcsecure too
    {
        playerAnswer = playerAnswer.ToLower();
       var h =  sentImagesandViews.Find(x => x.connectionID == player);
        if(playerAnswer == bytes[h.indexinlist].playerAnswer)
        {
            //let them do whatever
        }
        sentImagesandViews.Remove(h);
        //make sure their answer was the one we sent to this player
    }
    
}
public class CaptchaCtor
{
    public byte[] data;
    public string playerAnswer;
    public CaptchaCtor(byte[] data, string playerAnswer)
    {
        this.data = data;
        this.playerAnswer = playerAnswer;
    }
}
public class ViewsandIndexa{
    public int connectionID;
    public int indexinlist;

    public ViewsandIndexa(int player, int indexinlist)
    {
        this.connectionID = player;
        this.indexinlist = indexinlist;
       
    }
}