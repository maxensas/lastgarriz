using System;

namespace Lastgarriz.ViewModels.Command
{
    public class CompositeCommandParameter
    {
        public CompositeCommandParameter(EventArgs eventArgs, object parameter)
        {
            EventArgs = eventArgs;
            Parameter = parameter;
        }

        public EventArgs EventArgs { get; }

        public object Parameter { get; }
    }
}
