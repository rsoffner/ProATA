﻿using ProATA.SharedKernel;
using ProATA.SharedKernel.SignalProcessor;
using TaskProcessing.Core.Events;
using TaskProcessing.Core.Interfaces;

namespace TaskProcessing.Core.Models
{
    public sealed class APITask : Entity<Guid>
    {
         public string Title { get; set; }

        internal TaskState _currentState;
        internal IRunStrategy _runStrategy;

        public APITask(Guid id, string title) : base(id)
        {
            Id = id;    
            Title = title;

            _currentState = new ReadyState(this);
        }

        public void SetRunStrategy(IRunStrategy strategy)
        {
            _runStrategy = strategy;
        }

        public Task Run()
        {
            if (_runStrategy == null)
            {
                throw new InvalidOperationException("Run strategy must be set");
            }

            _currentState.Run();

            return Task.CompletedTask;
        }

        public Task End()
        {
            _currentState.End();

            return Task.CompletedTask;
        }

        public Task Enable()
        {
            _currentState.Enable();

            return Task.CompletedTask;
        }

        public Task Disable()
        {
            _currentState.Disable();

            return Task.CompletedTask;
        }

        public async Task _Run()
        {
            Events.Add(new EventMessage(this.Id, "Running"));
            await _runStrategy.Run();
         }

        public Task _End()
        {
            Events.Add(new EventMessage(this.Id, "Ready"));

            return Task.CompletedTask;
        }

        public Task _Enable()
        {
            Events.Add(new EventMessage(this.Id, "Enabled"));

            return Task.CompletedTask;
        }

        public Task _Disable()
        {
            Events.Add(new EventMessage(this.Id, "Disabled"));

            return Task.CompletedTask;
        }
    }
}