﻿namespace Common.BehaviourTrees
{
    public class BT_RaceNode : BT_ACompositeNode
    {
        private bool _ran;

        public BT_RaceNode() :
            base("Race")
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            _ran = false;
        }

        protected override BT_EStatus OnUpdate()
        {
            var status = BT_EStatus.Failure;

            for (int i = _current; i < _nodes.Length; ++i)
            {
                var current = _nodes[i];
                if (
                    current.Status == BT_EStatus.Running ||
                    !_ran
                )
                {
                    var result = current.WrappedExecute();

                    switch (result)
                    {
                        case BT_EStatus.Failure:
                            if (_current == i)
                            {
                                _current += 1;
                            }
                            break;

                        case BT_EStatus.Success:
                            status = BT_EStatus.Success;
                            break;

                        case BT_EStatus.Running:
                            if (status != BT_EStatus.Success)
                            {
                                status = BT_EStatus.Running;
                            }
                            break;
                    }
                }
            }

            _ran = true;

            return status;
        }

        protected override void OnFinish(BT_EStatus status)
        {
            base.OnFinish(status);

            AbortRunningTasks();
        }

        private void AbortRunningTasks()
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

            AbortRunningTasks();
        }
    }
}
