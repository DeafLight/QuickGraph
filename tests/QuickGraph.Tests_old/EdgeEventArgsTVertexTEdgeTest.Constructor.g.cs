// <copyright file="EdgeEventArgsTVertexTEdgeTest.Constructor.g.cs" company="MSIT">Copyright © MSIT 2007</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Engine.Exceptions;

namespace QuickGraph
{
    public partial class EdgeEventArgsTVertexTEdgeTest
    {
[TestMethod]
[Ignore]
[PexGeneratedBy(typeof(EdgeEventArgsTVertexTEdgeTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException944()
{
    try
    {
      EdgeEventArgs<int, Edge<int>> edgeEventArgs;
      edgeEventArgs = this.Constructor<int, Edge<int>>((Edge<int>)null);
      throw new AssertFailedException();
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[TestMethod]
[PexGeneratedBy(typeof(EdgeEventArgsTVertexTEdgeTest))]
public void Constructor571()
{
    Edge<int> edge;
    EdgeEventArgs<int, Edge<int>> edgeEventArgs;
    edge = EdgeFactory.Create(0, 0);
    edgeEventArgs = this.Constructor<int, Edge<int>>(edge);
    Assert.IsNotNull((object)edgeEventArgs);
    Assert.IsNotNull(edgeEventArgs.Edge);
}
[TestMethod]
[PexGeneratedBy(typeof(EdgeEventArgsTVertexTEdgeTest))]
public void Constructor57101()
{
    EdgeEventArgs<int, SEdge<int>> edgeEventArgs;
    edgeEventArgs = this.Constructor<int, SEdge<int>>(default(SEdge<int>));
    Assert.IsNotNull((object)edgeEventArgs);
}
    }
}
