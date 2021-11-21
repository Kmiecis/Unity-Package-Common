﻿namespace Common.BehaviourTrees
{
    public class BT_ParallelNode : BT_ACompositeNode
    {
        private int _current;
        private bool _ran;

        public BT_ParallelNode() :
            base("Parallel")
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            _current = 0;
            _ran = false;
        }

        protected override BT_EStatus OnUpdate()
        {
            var status = BT_EStatus.Success;

            for (int i = _current; i < _nodes.Length; ++i)
            {
                var current = _nodes[i];
                if (
                    current.Status == BT_EStatus.Running ||
                    !_ran
                )
                {
                    var result = current.DecoratedExecute();

                    switch (result)
                    {
                        case BT_EStatus.Failure:
                            status = BT_EStatus.Failure;
                            break;

                        case BT_EStatus.Success:
                            if (_current == i)
                            {
                                _current += 1;
                            }
                            break;

                        case BT_EStatus.Running:
                            if (status != BT_EStatus.Failure)
                            {
                                status = BT_EStatus.Running;
                            }
                            break;
                    }
                }
            }

            _ran = true;

            if (status == BT_EStatus.Failure)
            {
                AbortRunningNodes();
            }

            return status;
        }

        private void AbortRunningNodes()
        {
            for (int i = _current; i < _nodes.Length; ++i)
            {
                var current = _nodes[i];
                if (current.Status == BT_EStatus.Running)
                {
                    current.Abort();
                }
            }
        }
        
        public override void Abort()
        {
            base.Abort();

            AbortRunningNodes();
        }
    }
}
