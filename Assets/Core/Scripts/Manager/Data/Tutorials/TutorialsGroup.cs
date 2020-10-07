
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Utilities.Service.RFirebase;

namespace FoodZombie
{
    public class TutorialData : DataGroup
    {
        private BoolData mCompleted;
        private ListData<int> mFinishedSteps;

        public bool Completed { get { return mCompleted.Value; } }

        public TutorialData(int pId) : base(pId)
        {
            //mFinishedSteps = AddData(new ListData<int>(0, 0));
            mFinishedSteps = AddData(new ListData<int>(0, new List<int>()));
            mCompleted = AddData(new BoolData(1));
        }

        public void SaveStep(int pId)
        {
            if (!mFinishedSteps.Contain(pId))
                mFinishedSteps.Add(pId);
            GameData.Instance.SaveGame();
        }

        public bool IsStepFinished(int pId)
        {
            return mFinishedSteps.Contain(pId);
        }

        public void SetComplete(bool pValue)
        {
            mCompleted.Value=(pValue);

            //RFirebaseManager.LogEvent(TrackingConstants.EVENT_COMPLETE_TUTORIAL, TrackingConstants.PARAM_NAME, GetTrackingTutorialName());
            //RFirebaseManager.LogEvent(string.Format(TrackingConstants.EVENT_COMPLETE_TUTORIAL_, GetTrackingTutorialName()));
        }

        private string GetTrackingTutorialName()
        {
            switch (Id)
            {
                case TutorialsGroup.FIRST_MISSION_INTRO:
                    return "first mision intro";
                case TutorialsGroup.FIRST_MISSION:
                    return "first mission"; //ok
                case TutorialsGroup.SECOND_MISSION:
                    return "second mission"; //ok
                case TutorialsGroup.PERK:
                    return "perk"; //ok
                case TutorialsGroup.THIRD_MISSION:
                    return "third mission"; //ok
                case TutorialsGroup.PRE_UNIT:
                    return "pre unit"; //ok
                case TutorialsGroup.CLAIM_UNIT:
                    return "claim unit"; //ok
                case TutorialsGroup.UPGRADE_UNIT:
                    return "upgrade unit"; //ok
                case TutorialsGroup.WHEEL:
                    return "wheel"; //ok
                case TutorialsGroup.SHOP:
                    return "shop"; //ok
                case TutorialsGroup.UPGRADE_VEHICLE:
                    return "upgrade vehicle"; //ok
            }
            return "";
        }
    }

    public class TutorialsGroup : DataGroup
    {
        //NOTE: These below flows attually like Tooltips Chain than Tutorials
        //Player can mostly ignore them, but they will not disapear util some certain conditions are completed
        //And most importantly, multi Tooltips Chains can be active at the same time

        //Unignorable tutorial
        public const int FIRST_MISSION_INTRO = 0;
        public const int FIRST_MISSION = 1;//OK
        public const int SECOND_MISSION = 2;//OK
        public const int PERK = 3;//OK

        //These are tooltips so it is ignorable
        public const int PRE_UNIT = 4; //OK
        public const int UPGRADE_UNIT = 5; //OK
        public const int WHEEL = 6; //OK
        public const int CLAIM_UNIT = 7;//
        public const int SHOP = 9;//
        public const int UPGRADE_VEHICLE = 14;//

        public const int THIRD_MISSION = 16;//OK

        private DataGroup mGroup;

        public TutorialsGroup(int pId) : base(pId)
        {
            mGroup = AddData(new DataGroup(0));
            //
            mGroup.AddData(new TutorialData(FIRST_MISSION_INTRO));
            mGroup.AddData(new TutorialData(FIRST_MISSION));
            mGroup.AddData(new TutorialData(SECOND_MISSION));
            mGroup.AddData(new TutorialData(PERK));
            //
            mGroup.AddData(new TutorialData(PRE_UNIT));
            mGroup.AddData(new TutorialData(CLAIM_UNIT));
            mGroup.AddData(new TutorialData(UPGRADE_UNIT));
            mGroup.AddData(new TutorialData(WHEEL));
            mGroup.AddData(new TutorialData(SHOP));
            mGroup.AddData(new TutorialData(UPGRADE_VEHICLE));
            mGroup.AddData(new TutorialData(THIRD_MISSION));
        }

        public void Finish(int pTutId)
        {
            foreach (TutorialData t in mGroup.Children)
                if (t.Id == pTutId)
                {
                    t.SetComplete(true);
                    EventDispatcher.Raise(new TutorialFinishedEvent(pTutId));
                }
        }

        public void Reset(int pTutId)
        {
            foreach (TutorialData t in mGroup.Children)
                if (t.Id == pTutId)
                    t.Reset();
        }

        public bool IsFinished(int pTutId)
        {
            foreach (TutorialData t in mGroup.Children)
                if (t.Id == pTutId)
                    return t.Completed;
            return true;
        }

        public List<TutorialData> GetTutorialsCompleted()
        {
            var list = new List<TutorialData>();
            foreach (TutorialData t in mGroup.Children)
            {
                if (t.Completed)
                    list.Add(t);
            }
            return list;
        }

        public TutorialData GetTutorial(int pTutId)
        {
            foreach (TutorialData t in mGroup.Children)
                if (t.Id == pTutId)
                    return t;
            return null;
        }
    }
}