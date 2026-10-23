using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{ 
    public interface ISillyTavernModel : IModel
    {
        BindableProperty<int> curHeroid { get; }
        BindableList<RoleInfo> CurHeros { get; set; }
        Dictionary<int,BindableList<RoleInfo>> mHeros {  get; }
        Dictionary<int,float> LastRefreshTime { get;}
        bool HaveHero(int id);
        void SetCurHeros(int id);
        RoleInfo Remove(int SillyTavernid,int id);
        RoleInfo GetHero(int id);
        void Refresh(int SillyTavernid,List<RoleInfo> roles);

        float GetFreshTime(int id);
        void SetFreshTime(int id, float time);
    }
    public class SillyTavernModel : AbstractModel, ISillyTavernModel
    {
        public Dictionary<int, BindableList<RoleInfo>> mHeros { get; } = new Dictionary<int, BindableList<RoleInfo>>();

        public BindableList<RoleInfo> CurHeros { get; set; } =new BindableList<RoleInfo>();
        
        public BindableProperty<int> curHeroid { get; }=new BindableProperty<int>();
        public Dictionary<int, float> LastRefreshTime { get;} = new();
        public bool HaveHero(int id)
        {
            foreach (var item in CurHeros)
            {
                if(item.Id==id)
                    return false;
            }
            return true;
        }
        public void SetCurHeros(int id)
        {
            CurHeros= mHeros[id];
        }
        public void Refresh(int SillyTavernid, List<RoleInfo> roles)
        {
            if (!mHeros.TryGetValue(SillyTavernid, out var list))
            {
                list = new BindableList<RoleInfo>();
                mHeros[SillyTavernid] = list;
            }
            else
            {
                list.Clear();
            }

            foreach (var role in roles)
            {
                if(curHeroid.Value==0)
                    curHeroid.Value = role.Id;
                list.Add(role);
            }
                
        }
        public RoleInfo Remove(int SillyTavernid, int id)
        {
            if(mHeros.TryGetValue(SillyTavernid,out var roleInfos))
            {
                for (int i = 0; i < roleInfos.Count; i++)
                {
                    if (roleInfos[i].Id == id)
                    {
                        var target = roleInfos[i];
                        roleInfos.RemoveAt(i);
                        return target;
                    }        
                }
            }
            return null;
        }

        public RoleInfo GetHero(int id)
        {

            foreach (var hero in CurHeros)
                if (hero.Id == id) return hero;
            return null;
        }

        public float GetFreshTime(int id)
        {
            return LastRefreshTime.TryGetValue(id, out var time) ? time : 0f;
        }
        public void SetFreshTime(int id, float time)
        {
            LastRefreshTime[id] = time;
        }
        protected override void OnInit()
        {
            
        }
    }
}
