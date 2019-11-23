/*
    The MIT License (MIT)

    Copyright (c) 2017 Stéphane Lepin

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;

namespace OBSWebsocketDotNet
{

    /// <summary>
    /// Called by <see cref="OBSWebSocket.SceneChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="newSceneName">Name of the new current scene</param>
    public delegate void SceneChangeCallback(OBSWebSocket sender, string newSceneName);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.SourceOrderChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sceneName">Name of the scene where items where reordered</param>
    public delegate void SourceOrderChangeCallback(OBSWebSocket sender, string sceneName);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.SceneItemVisibilityChanged"/>, <see cref="OBSWebSocket.SceneItemAdded"/>
    /// or <see cref="OBSWebSocket.SceneItemRemoved"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sceneName">Name of the scene where the item is</param>
    /// <param name="itemName">Name of the concerned item</param>
    public delegate void SceneItemUpdateCallback(OBSWebSocket sender, string sceneName, string itemName);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.TransitionChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="newTransitionName">Name of the new selected transition</param>
    public delegate void TransitionChangeCallback(OBSWebSocket sender, string newTransitionName);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.TransitionDurationChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="newDuration">Name of the new transition duration (in milliseconds)</param>
    public delegate void TransitionDurationChangeCallback(OBSWebSocket sender, int newDuration);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.StreamingStateChanged"/>, <see cref="OBSWebSocket.RecordingStateChanged"/>
    /// or <see cref="OBSWebSocket.ReplayBufferStateChanged"/> 
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="type">New output state</param>
    public delegate void OutputStateCallback(OBSWebSocket sender, OutputState type);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.StreamStatus"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="status">Stream status data</param>
    public delegate void StreamStatusCallback(OBSWebSocket sender, StreamStatus status);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.StudioModeSwitched"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="enabled">New Studio Mode status</param>
    public delegate void StudioModeChangeCallback(OBSWebSocket sender, bool enabled);

    /// <summary>
    /// Called by <see cref="OBSWebSocket.Heartbeat"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="heatbeat">heartbeat data</param>
    public delegate void HeartBeatCallback(OBSWebSocket sender, Heartbeat heatbeat);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SceneItemDeselected"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sceneName">Name of the scene item was in</param>
    /// <param name="itemName">Name of the item deselected</param>
    /// <param name="itemId">Id of the item deselected</param>
    public delegate void SceneItemDeselectedCallback(OBSWebSocket sender, string sceneName, string itemName, string itemId);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SceneItemSelected"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sceneName">Name of the scene item was in</param>
    /// <param name="itemName">Name of the item seletected</param>
    /// <param name="itemId">Id of the item selected</param>
    public delegate void SceneItemSelectedCallback(OBSWebSocket sender, string sceneName, string itemName, string itemId);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SceneItemTransformChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="transform">Transform data</param>
    public delegate void SceneItemTransformCallback(OBSWebSocket sender, SceneItemTransformInfo transform);


    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceAudioMixersChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="mixerInfo">Mixer information that was updated</param>
    public delegate void SourceAudioMixersChangedCallback(OBSWebSocket sender, AudioMixersChangedInfo mixerInfo);



    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceAudioSyncOffsetChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of the source for the offset change</param>
    /// <param name="syncOffset">Sync offset value</param>
    public delegate void SourceAudioSyncOffsetCallback(OBSWebSocket sender, string sourceName, int syncOffset);


    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceCreated"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="settings">Newly created source settings</param>
    public delegate void SourceCreatedCallback(OBSWebSocket sender, SourceSettings settings);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceDestroyed"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Newly destroyed source information</param>
    /// <param name="sourceKind">Kind of source destroyed</param>
    /// <param name="sourceType">Type of source destroyed</param>
    public delegate void SourceDestroyedCallback(OBSWebSocket sender, string sourceName, string sourceType, string sourceKind);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceRenamed"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="newName">New name of source</param>
    /// <param name="previousName">Previous name of source</param>
    public delegate void SourceRenamedCallback(OBSWebSocket sender, string newName, string previousName);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceMuteStateChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of source</param>
    /// <param name="muted">Current mute state of source</param>
    public delegate void SourceMuteStateChangedCallback(OBSWebSocket sender, string sourceName, bool muted);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceVolumeChanged"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of source</param>
    /// <param name="volume">Current volume level of source</param>
    public delegate void SourceVolumeChangedCallback(OBSWebSocket sender, string sourceName, float volume);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceFilterRemoved"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of source</param>
    /// <param name="filterName">Name of removed filter</param>
    public delegate void SourceFilterRemovedCallback(OBSWebSocket sender, string sourceName, string filterName);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceFilterAdded"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of source</param>
    /// <param name="filterName">Name of filter</param>
    /// <param name="filterType">Type of filter</param>
    /// <param name="filterSettings">Settings for filter</param>
    public delegate void SourceFilterAddedCallback(OBSWebSocket sender, string sourceName, string filterName, string filterType, JObject filterSettings);

    /// <summary>
    /// Callback by <see cref="OBSWebSocket.SourceFiltersReordered"/>
    /// </summary>
    /// <param name="sender"><see cref="OBSWebSocket"/> instance</param>
    /// <param name="sourceName">Name of source</param>
    /// <param name="filters">Current order of filters for source</param>
    public delegate void SourceFiltersReorderedCallback(OBSWebSocket sender, string sourceName, List<FilterReorderItem> filters);

    /// <summary>
    /// Thrown if authentication fails
    /// </summary>
    public class AuthFailureException : Exception
    {
    }

    /// <summary>
    /// Thrown when the server responds with an error
    /// </summary>
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public ErrorResponseException(string message) : base(message)
        {
        }
    }
}
