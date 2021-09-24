using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Dialog
{
    public int Index { get; set; }
    public string Speaker { get; set; }
    public string Txt { get; set; }
    public int Portrait1 { get; set; }
    public int Portrait2 { get; set; }
}

public class Obj
{
    public int Index { get; set; }
    public string Txt { get; set; }
}

public class Mail
{
    public int Index { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string Date { get; set; }
    public string Content { get; set; }
}

public class Card
{
    public int Index { get; set; }
    public string Btn1 { get; set; }
    public string Btn2 { get; set; }
    public string Btn3 { get; set; }
    public string Btn4 { get; set; }
}

public class SystemMsg
{
    public int Index { get; set; }
    public string Txt { get; set; }
}

public class ItemInfo
{
    public int Index { get; set; }
    public string Txt { get; set; }
}
//==========================================================
public class CSVManager : MonoBehaviour
{
    Dictionary<int, Dictionary<int, Dictionary<int, Dialog>>> sceneDic;
    Dictionary<int, Dictionary<int, Dictionary<int, Obj>>> objDic;
    Dictionary<int, Dictionary<int, Dictionary<int, SystemMsg>>> systemDic;
    Dictionary<int, ItemInfo> itemDic;
    Dictionary<int, Dictionary<int, Mail>> mailDic;
    //List<object> list;

    private void Awake() {
        sceneDic = new Dictionary<int, Dictionary<int, Dictionary<int, Dialog>>>();
        objDic = new Dictionary<int, Dictionary<int, Dictionary<int, Obj>>>();
        systemDic = new Dictionary<int, Dictionary<int, Dictionary<int, SystemMsg>>>();
        itemDic = new Dictionary<int, ItemInfo>();
        mailDic = new Dictionary<int, Dictionary<int, Mail>>();
        //list = new List<object>();
    }

    //------------------------------------------------------
    //** 대화 **
    public void SetDialog()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Dialog");
        string content = textAsset.text;

        // ** 씬 구분 (/)
        int sceneIndex = 0;
        string[] scene = content.Split('/');
        for (int m = 1; m < scene.Length; m++){
            // ** 컷 구분 (&)
            int cutIndex = 0;
            string[] cut = scene[m].Split('&');
            var cutDic = new Dictionary<int, Dictionary<int, Dialog>>();
            for (int j = 0; j < cut.Length; j++) {
                // ** 라인 구분 (엔터)
                string[] line = cut[j].Split('\n');
                var DialogDic = new Dictionary<int, Dialog>();

                for (int i = 0; i < line.Length; i++) {
                    // ** 속성 구분
                    string[] column = line[i].Split('@');
                    if (column.Length < 5)
                        continue;

                    int index = 0;
                    Dialog dialog = new Dialog();
                    dialog.Index = int.Parse(column[index++]);
                    dialog.Speaker = column[index++];
                    dialog.Txt = column[index++];
                    dialog.Portrait1 = int.Parse(column[index++]);
                    dialog.Portrait2 = int.Parse(column[index++]);

                    DialogDic.Add(dialog.Index, dialog);
                }
                    
                cutDic.Add(cutIndex, DialogDic);
                cutIndex += 10;
            }

            sceneDic.Add(sceneIndex, cutDic);
            sceneIndex += 100;
        }

        //list.Add(sceneDic);
    }

    public Dialog GetDialog(int _sceneIndex, int _cutIndex, int _index)
    {
        if (sceneDic[_sceneIndex][_cutIndex].ContainsKey(_index)) {
            return sceneDic[_sceneIndex][_cutIndex][_index];
        }

        return null;
    }

    //------------------------------------------------------
    //** 오브젝트 상호작용 **
    public void SetObjDialog()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Obj");
        string content = textAsset.text;

        // ** 씬 구분 (/)
        int sceneIndex = 0;
        string[] scene = content.Split('/');
        for (int m = 1; m < scene.Length; m++){
            // ** 컷 구분 (&)
            int cutIndex = 0;
            string[] cut = scene[m].Split('&');
            var cutDic = new Dictionary<int, Dictionary<int, Obj>>();
            for (int j = 0; j < cut.Length; j++) {
                // ** 라인 구분 (엔터)
                string[] line = cut[j].Split('\n');
                var dic = new Dictionary<int, Obj>();

                for (int i = 0; i < line.Length; i++) {
                    // ** 속성 구분
                    string[] column = line[i].Split('@');
                    if (column.Length < 2)
                        continue;

                    int index = 0;
                    Obj obj = new Obj();
                    obj.Index = int.Parse(column[index++]);
                    obj.Txt = column[index++];

                    dic.Add(obj.Index, obj);
                }
                    
                cutDic.Add(cutIndex, dic);
                cutIndex += 10;
            }

            objDic.Add(sceneIndex, cutDic);
            sceneIndex += 100;
        }

        //list.Add(objDic);
    }

    public Obj GetObj(int _objIndex, int _cutIndex, int _index)
    {
         if (objDic[_objIndex][_cutIndex].ContainsKey(_index)) {
            return objDic[_objIndex][_cutIndex][_index];
        }

        return null;
    }

    //------------------------------------------------------
    public void SetSystemDialog()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/SystemMsg");
        string content = textAsset.text;

        // ** 씬 구분 (/)
        int sceneIndex = 0;
        string[] scene = content.Split('/');
        for (int m = 1; m < scene.Length; m++){
            // ** 컷 구분 (&)
            int cutIndex = 0;
            string[] cut = scene[m].Split('&');
            var cutDic = new Dictionary<int, Dictionary<int, SystemMsg>>();
            for (int j = 0; j < cut.Length; j++) {
                // ** 라인 구분 (엔터)
                string[] line = cut[j].Split('\n');
                var dic = new Dictionary<int, SystemMsg>();
                for (int i = 0; i < line.Length; i++) {
                    // ** 속성 구분
                    string[] column = line[i].Split('@');
                    if (column.Length < 2)
                        continue;

                    int index = 0;
                    SystemMsg sm = new SystemMsg();
                    sm.Index = int.Parse(column[index++]);
                    sm.Txt = column[index++];

                    dic.Add(sm.Index, sm);
                }
                    
                cutDic.Add(cutIndex, dic);
                cutIndex += 10;
            }

            systemDic.Add(sceneIndex, cutDic);
            sceneIndex += 100;
        }

        //list.Add(objDic);
    }

    public SystemMsg GetSystem(int _sceneIndex, int _cutIndex, int _index)
    {
         if (systemDic[_sceneIndex][_cutIndex].ContainsKey(_index)) {
            return systemDic[_sceneIndex][_cutIndex][_index];
        }

        return null;
    }
    //------------------------------------------------------
    public void SetItemText()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/ItemInfo");
        string content = textAsset.text;
        
        string[] line = content.Split('\n');
        for (int i = 1; i < line.Length; i++) {
            // ** 속성 구분
            string[] column = line[i].Split('@');
            if (column.Length < 2)
                continue;

            int index = 0;
            ItemInfo item = new ItemInfo();
            item.Index = int.Parse(column[index++]);
            item.Txt = column[index++];

            itemDic.Add(item.Index, item);
        }
    }

    public string GetItemInfo(int _index)
    {
         if (itemDic.ContainsKey(_index)) {
            return itemDic[_index].Txt;
        }

        return null;
    }
    //------------------------------------------------------
    public void SetMailText()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CSV/Mail");
        string content = textAsset.text;
        
        int cutIndex = 0;
        string[] cut = content.Split('/');
        for (int j = 1; j < cut.Length; j++) {
            string[] line = cut[j].Split('\n');
            var dic = new Dictionary<int, Mail>();
            for (int i = 1; i < line.Length; i++) {
                // ** 속성 구분
                string[] column = line[i].Split('@');
                if (column.Length < 5)
                    continue;

                int index = 0;
                Mail mail = new Mail();
                mail.Index = int.Parse(column[index++]);
                mail.Name = column[index++];
                mail.Title = column[index++];
                mail.Date = column[index++];
                mail.Content = column[index++];

                dic.Add(mail.Index, mail);
            }
            mailDic.Add(cutIndex, dic);
            cutIndex += 100;
        }
    }

    public Mail GetMailInfo(int _cutIndex, int _index)
    {
         if (mailDic[_cutIndex].ContainsKey(_index)) {
            return mailDic[_cutIndex][_index];
        }

        return null;
    }
}
