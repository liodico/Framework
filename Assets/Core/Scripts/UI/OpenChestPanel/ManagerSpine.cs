using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System;
using UnityEngine;

namespace FoodZombie.UI
{
    public class ManagerSpine : MonoBehaviour
    {
        SkeletonAnimation skeAnim;
        SkeletonGraphic skeGraph;

        Spine.AnimationState anim;
        Skeleton ske;

        Action<string>[] eventCallback = new Action<string>[5];
        Action[] doneCallback = new Action[5];

        int status = 0;
        int Status
        {
            get
            {
                if (status == 0)
                {
                    skeAnim = GetComponent<SkeletonAnimation>();
                    if (skeAnim is null)
                    {
                        skeGraph = GetComponent<SkeletonGraphic>();
                        if (skeGraph is null) status = -1;
                        else
                        {
                            anim = skeGraph.AnimationState;
                            ske = skeGraph.Skeleton;
                            skeGraph.AnimationState.Event += RaiseAnimEvent;
                            skeGraph.AnimationState.Complete += RaiseDoneEvent;
                            status = 2;
                        }
                    }
                    else
                    {
                        anim = skeAnim.AnimationState;
                        ske = skeAnim.Skeleton;
                        skeAnim.AnimationState.Event += RaiseAnimEvent;
                        skeAnim.AnimationState.Complete += RaiseDoneEvent;
                        status = 1;
                    }
                    return status;
                }
                else return status;
            }
        }

        public float TimeScale
        {
            get
            {
                if (Status == 1) return skeAnim.timeScale;
                else if (Status == 2) return skeGraph.timeScale;
                else return 1;
            }
            set
            {
                if (Status == 1) skeAnim.timeScale = value;
                else if (Status == 2) skeGraph.timeScale = value;
            }
        }

        public string AnimName(int track = 0)
        {
            if (Status > 0) return anim.Tracks.Items[track].Animation.Name;
            else return "";
        }

        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
        }

        public void Clear(int t = -1)
        {
            if (Status < 0) return;
            if (t > -1)
            {
                if (t < 5)
                {
                    eventCallback[t] = null;
                    doneCallback[t] = null;
                }
                anim.SetEmptyAnimation(t, 0);
            }
            else
            {
                for (int i = 0; i < 5; ++i)
                {
                    eventCallback[i] = null;
                    doneCallback[i] = null;
                    anim.SetEmptyAnimation(i, 0);
                }
            }
        }

        public void PlayAnim(string animName, bool loop = true, bool playAgain = true, int track = 0)
        {
            if (Status < 0) return;
            if (!playAgain && AnimName(track) == animName) return;

            SetActive(true);
            anim.SetAnimation(track, animName, loop);
            doneCallback[track] = null;
            eventCallback[track] = null;
        }

        public void SetDoneEvent(Action act, int track = 0)
        {
            if (track < doneCallback.Length)
            {
                doneCallback[track] = act;
            }
        }

        public void SetAnimEvent(Action<string> act, int track = 0)
        {
            if (track < eventCallback.Length)
            {
                eventCallback[track] = act;
            }
        }

        void RaiseDoneEvent(TrackEntry trackE)
        {
            if (trackE.Animation.Name != AnimName(trackE.TrackIndex)) return;
            doneCallback[trackE.TrackIndex]?.Invoke();
        }

        void RaiseAnimEvent(TrackEntry trackE, Spine.Event e)
        {
            eventCallback[trackE.TrackIndex]?.Invoke(e.Data.Name);
        }

        public void SetSkin(string name)
        {
            if (Status < 0) return;
            SetSkin(ske.Data.FindSkin(name));
        }

        public void SetSkin(Skin skin)
        {
            if (Status < 0) return;
            ske.SetSkin(skin);
            ske.SetSlotsToSetupPose();
            anim.Apply(ske);
        }

        public int GetNumOfEvent(string name, int track = 0)
        {
            if (Status < 0) return -1;
            var a = ske.Data.FindAnimation(AnimName(track));
            var t = 0;
            foreach (var time in a.Timelines)
            {
                if (time is EventTimeline)
                {
                    foreach (var e in (time as EventTimeline).Events)
                    {
                        if (e.Data.Name == name) ++t;
                    }
                }
            }
            return t;
        }

        public float GetTimeOfEvent(string name, int track = 0)
        {
            if (Status < 0) return -1;
            var a = ske.Data.FindAnimation(AnimName(track));
            foreach (var time in a.Timelines)
            {
                if (time is EventTimeline)
                {
                    foreach (var e in (time as EventTimeline).Events)
                    {
                        if (e.Data.Name == name) return e.Time;
                    }
                }
            }
            return -1;
        }

        public void SetTime(int track, float time)
        {
            if (Status < 0) return;
            anim.Tracks.Items[track].TrackTime = time;
        }

        public void SetAttachment(string slot, string name)
        {
            if (Status < 0) return;
            ske.SetAttachment(slot, name);
        }

        public void ChangeAssets(string slot, string name, Sprite spr)
        {
            if (Status < 0) return;
            var dataAsset = Status == 1 ? skeAnim.SkeletonDataAsset : skeGraph.SkeletonDataAsset;
            var slotIndex = ske.FindSlotIndex(slot);
            var sourceMaterial = dataAsset.atlasAssets[0].PrimaryMaterial;

            Attachment attachment = ske.Skin.GetAttachment(slotIndex, name).GetRemappedClone(spr, sourceMaterial, true, true, true);
            var tempSkin = new Skin("Temp");
            tempSkin.SetAttachment(slotIndex, name, attachment);

            var packSkin = new Skin("Pack");
            packSkin.AddSkin(ske.Data.DefaultSkin);
            packSkin.AddSkin(ske.Skin);
            packSkin.AddSkin(tempSkin);

            Material runtimeMaterial;
            Texture2D runtimeAtlas;

            SetSkin(packSkin.GetRepackedSkin("Repacked", sourceMaterial, out runtimeMaterial, out runtimeAtlas));
        }
    }
}
