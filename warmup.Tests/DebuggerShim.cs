using System;
using NSpec.Domain;
using System.Reflection;
using NSpec;
using System.Linq;

public class DebuggerShim
{
    //tests can be run via TDD.net
    public void debug()
    {
        var tagOrClassName = "";

        var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location, tagOrClassName);

        var contexts = invocation.Run();

        contexts.Failures().Count().should_be(0);
    }
}
