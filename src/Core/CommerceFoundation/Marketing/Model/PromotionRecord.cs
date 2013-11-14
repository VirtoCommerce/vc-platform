using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Marketing.Model
{
    public class PromotionRecord
    {
        private string _PromotionType;

        public string PromotionType
        {
            get { return _PromotionType; }
            set { _PromotionType = value; }
        }

        private PromotionEntrySet _TargetEntriesSet;

        public PromotionEntrySet TargetEntriesSet
        {
            get { return _TargetEntriesSet; }
            set { _TargetEntriesSet = value; }
        }
        
        private PromotionEntrySet _AffectedEntriesSet;

        public PromotionEntrySet AffectedEntriesSet
        {
            get { return _AffectedEntriesSet; }
            set { _AffectedEntriesSet = value; }
        }
        
        private PromotionReward _Reward;

        public PromotionReward Reward
        {
            get { return _Reward; }
            set { _Reward = value; }
        }

        public PromotionRecord()
        {
        }
    }
}
