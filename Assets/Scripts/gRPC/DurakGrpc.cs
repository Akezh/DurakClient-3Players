// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/durak.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace DurakServer {
  public static partial class DurakGame
  {
    static readonly string __ServiceName = "durak.DurakGame";

    static readonly grpc::Marshaller<global::DurakServer.DurakRequest> __Marshaller_durak_DurakRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::DurakServer.DurakRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::DurakServer.DurakReply> __Marshaller_durak_DurakReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::DurakServer.DurakReply.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::DurakServer.TimerRequest> __Marshaller_durak_TimerRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::DurakServer.TimerRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::DurakServer.TimerReply> __Marshaller_durak_TimerReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::DurakServer.TimerReply.Parser.ParseFrom);

    static readonly grpc::Method<global::DurakServer.DurakRequest, global::DurakServer.DurakReply> __Method_DurakStreaming = new grpc::Method<global::DurakServer.DurakRequest, global::DurakServer.DurakReply>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "DurakStreaming",
        __Marshaller_durak_DurakRequest,
        __Marshaller_durak_DurakReply);

    static readonly grpc::Method<global::DurakServer.TimerRequest, global::DurakServer.TimerReply> __Method_StartTimerStreaming = new grpc::Method<global::DurakServer.TimerRequest, global::DurakServer.TimerReply>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "StartTimerStreaming",
        __Marshaller_durak_TimerRequest,
        __Marshaller_durak_TimerReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::DurakServer.DurakReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for DurakGame</summary>
    public partial class DurakGameClient : grpc::ClientBase<DurakGameClient>
    {
      /// <summary>Creates a new client for DurakGame</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DurakGameClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DurakGame that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DurakGameClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DurakGameClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DurakGameClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncDuplexStreamingCall<global::DurakServer.DurakRequest, global::DurakServer.DurakReply> DurakStreaming(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DurakStreaming(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::DurakServer.DurakRequest, global::DurakServer.DurakReply> DurakStreaming(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_DurakStreaming, null, options);
      }
      public virtual grpc::AsyncServerStreamingCall<global::DurakServer.TimerReply> StartTimerStreaming(global::DurakServer.TimerRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return StartTimerStreaming(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncServerStreamingCall<global::DurakServer.TimerReply> StartTimerStreaming(global::DurakServer.TimerRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_StartTimerStreaming, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DurakGameClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DurakGameClient(configuration);
      }
    }

  }
}
#endregion
