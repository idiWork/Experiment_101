using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppServiceBusReceive.Blockchain
{
    public class Action
    {
        public int workflowFunctionId { get; set; }
        public List<Workflowactionparameter> workflowActionParameters { get; set; }

        public Action() {}

        public Action(int functionId)
        {
            workflowFunctionId = functionId;
            workflowActionParameters = new List<Workflowactionparameter>();
        }

        public Action(int functionId, int temperature)
        {
            workflowFunctionId = functionId;
            workflowActionParameters = new List<Workflowactionparameter>();
            workflowActionParameters.Add(new Workflowactionparameter(temperature));
        }
    }

    public class Workflowactionparameter
    {
        public string name { get; set; }
        public int value { get; set; }

        public Workflowactionparameter() {}

        public Workflowactionparameter(int temperature)
        {
            name = "temperature";
            value = temperature;
        }
    }
}
