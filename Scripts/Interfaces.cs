using System;

namespace ComparativeAdvantage
{
    interface IHasDialogLabel
    {
        void SetLabel( String label );
    }
    interface IScenarioVisualsOrUI
    {
        void OnDialogVisualsEvent( String visualsId );
    }
}