using System;
using System.Windows.Forms;
using OBS.WebSocket.NET;
using OBS.WebSocket.NET.Types;

namespace TestClient
{
    public partial class MainWindow : Form
    {
        protected ObsWebSocket _obs;

        public MainWindow()
        {
            InitializeComponent();
            _obs = new ObsWebSocket();

            _obs.Connected += onConnect;
            _obs.Disconnected += onDisconnect;

            _obs.SceneChanged += onSceneChange;
            _obs.SceneCollectionChanged += onSceneColChange;
            _obs.ProfileChanged += onProfileChange;
            _obs.TransitionChanged += onTransitionChange;
            _obs.TransitionDurationChanged += onTransitionDurationChange;

            _obs.StreamingStateChanged += onStreamingStateChange;
            _obs.RecordingStateChanged += onRecordingStateChange;

            _obs.StreamStatus += onStreamData;
        }

        private void onConnect(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() => {
                txtServerIP.Enabled = false;
                txtServerPassword.Enabled = false;
                btnConnect.Text = "Disconnect";

                gbControls.Enabled = true;

                var versionInfo = _obs.GetVersion();
                tbPluginVersion.Text = versionInfo.PluginVersion;
                tbOBSVersion.Text = versionInfo.OBSStudioVersion;

                btnListScenes.PerformClick();
                btnGetCurrentScene.PerformClick();

                btnListSceneCol.PerformClick();
                btnGetCurrentSceneCol.PerformClick();

                btnListProfiles.PerformClick();
                btnGetCurrentProfile.PerformClick();

                btnListTransitions.PerformClick();
                btnGetCurrentTransition.PerformClick();

                btnGetTransitionDuration.PerformClick();

                var streamStatus = _obs.Api.GetStreamingStatus();
                if (streamStatus.IsStreaming)
                    onStreamingStateChange(_obs, OutputState.Started);
                else
                    onStreamingStateChange(_obs, OutputState.Stopped);

                if (streamStatus.IsRecording)
                    onRecordingStateChange(_obs, OutputState.Started);
                else
                    onRecordingStateChange(_obs, OutputState.Stopped);
            }));
        }

        private void onDisconnect(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)(() => {
                gbControls.Enabled = false;

                txtServerIP.Enabled = true;
                txtServerPassword.Enabled = true;
                btnConnect.Text = "Connect";
            }));
        }

        private void onSceneChange(ObsWebSocket sender, string newSceneName)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                tbCurrentScene.Text = newSceneName;
            });
        }

        private void onSceneColChange(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                tbSceneCol.Text = _obs.Api.GetCurrentSceneCollection();
            });
        }

        private void onProfileChange(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                tbProfile.Text = _obs.Api.GetCurrentProfile();
            });
        }

        private void onTransitionChange(ObsWebSocket sender, string newTransitionName)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                tbTransition.Text = newTransitionName;
            });
        }

        private void onTransitionDurationChange(ObsWebSocket sender, int newDuration)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                tbTransitionDuration.Value = newDuration;
            });
        }

        private void onStreamingStateChange(ObsWebSocket sender, OutputState newState)
        {
            string state = "";
            switch(newState)
            {
                case OutputState.Starting:
                    state = "Stream starting...";
                    break;

                case OutputState.Started:
                    state = "Stop streaming";
                    BeginInvoke((MethodInvoker)delegate
                    {
                        gbStatus.Enabled = true;
                    });
                    break;

                case OutputState.Stopping:
                    state = "Stream stopping...";
                    break;

                case OutputState.Stopped:
                    state = "Start streaming";
                    BeginInvoke((MethodInvoker)delegate
                    {
                        gbStatus.Enabled = false;
                    });
                    break;

                default:
                    state = "State unknown";
                    break;
            }

            BeginInvoke((MethodInvoker)delegate
            {
                btnToggleStreaming.Text = state;
            });
        }

        private void onRecordingStateChange(ObsWebSocket sender, OutputState newState)
        {
            string state = "";
            switch (newState)
            {
                case OutputState.Starting:
                    state = "Recording starting...";
                    break;

                case OutputState.Started:
                    state = "Stop recording";
                    break;

                case OutputState.Stopping:
                    state = "Recording stopping...";
                    break;

                case OutputState.Stopped:
                    state = "Start recording";
                    break;

                default:
                    state = "State unknown";
                    break;
            }

            BeginInvoke((MethodInvoker)delegate
            {
                btnToggleRecording.Text = state;
            });
        }

        private void onStreamData(ObsWebSocket sender, StreamStatus data)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                txtStreamTime.Text = data.TotalStreamTime.ToString() + " sec";
                txtKbitsSec.Text = data.KbitsPerSec.ToString() + " kbit/s";
                txtBytesSec.Text = data.BytesPerSec.ToString() + " bytes/s";
                txtFramerate.Text = data.FPS.ToString() + " FPS";
                txtStrain.Text = (data.Strain * 100).ToString() + " %";
                txtDroppedFrames.Text = data.DroppedFrames.ToString();
                txtTotalFrames.Text = data.TotalFrames.ToString();
            });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(!_obs.IsConnected)
            {
                try
                {
                    _obs.Connect(txtServerIP.Text, txtServerPassword.Text);
                }
                catch (AuthFailureException)
                {
                    MessageBox.Show("Authentication failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                catch (ErrorResponseException ex)
                {
                    MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            } else
            {
                _obs.Disconnect();
            }
        }

        private void btnListScenes_Click(object sender, EventArgs e)
        {
            var scenes = _obs.Api.ListScenes();

            tvScenes.Nodes.Clear();
            foreach(var scene in scenes)
            {
                var node = new TreeNode(scene.Name);
                foreach (var item in scene.Items)
                {
                    node.Nodes.Add(item.SourceName);
                }

                tvScenes.Nodes.Add(node);
            }
        }

        private void btnGetCurrentScene_Click(object sender, EventArgs e)
        {
            tbCurrentScene.Text = _obs.Api.GetCurrentScene().Name;
        }

        private void btnSetCurrentScene_Click(object sender, EventArgs e)
        {
            _obs.Api.SetCurrentScene(tbCurrentScene.Text);
        }

        private void tvScenes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                tbCurrentScene.Text = e.Node.Text;
            }
        }

        private void btnListSceneCol_Click(object sender, EventArgs e)
        {
            var sc = _obs.Api.ListSceneCollections();

            tvSceneCols.Nodes.Clear();
            foreach (var sceneCol in sc)
            {
                tvSceneCols.Nodes.Add(sceneCol);
            }
        }

        private void btnGetCurrentSceneCol_Click(object sender, EventArgs e)
        {
            tbSceneCol.Text = _obs.Api.GetCurrentSceneCollection();
        }

        private void btnSetCurrentSceneCol_Click(object sender, EventArgs e)
        {
            _obs.Api.SetCurrentSceneCollection(tbSceneCol.Text);
        }

        private void tvSceneCols_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                tbSceneCol.Text = e.Node.Text;
            }
        }

        private void btnListProfiles_Click(object sender, EventArgs e)
        {
            var profiles = _obs.Api.ListProfiles();

            tvProfiles.Nodes.Clear();
            foreach (var profile in profiles)
            {
                tvProfiles.Nodes.Add(profile);
            }
        }

        private void btnGetCurrentProfile_Click(object sender, EventArgs e)
        {
            tbProfile.Text = _obs.Api.GetCurrentProfile();
        }

        private void btnSetCurrentProfile_Click(object sender, EventArgs e)
        {
            _obs.Api.SetCurrentProfile(tbProfile.Text);
        }

        private void tvProfiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                tbProfile.Text = e.Node.Text;
            }
        }

        private void btnToggleStreaming_Click(object sender, EventArgs e)
        {
            _obs.Api.ToggleStreaming();
        }

        private void btnToggleRecording_Click(object sender, EventArgs e)
        {
            _obs.Api.ToggleRecording();
        }

        private void btnListTransitions_Click(object sender, EventArgs e)
        {
            var transitions = _obs.Api.ListTransitions();

            tvTransitions.Nodes.Clear();
            foreach (var transition in transitions)
            {
                tvTransitions.Nodes.Add(transition);
            }
        }

        private void btnGetCurrentTransition_Click(object sender, EventArgs e)
        {
            tbTransition.Text = _obs.Api.GetCurrentTransition().Name;
        }

        private void btnSetCurrentTransition_Click(object sender, EventArgs e)
        {
            _obs.Api.SetCurrentTransition(tbTransition.Text);
        }

        private void tvTransitions_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                tbTransition.Text = e.Node.Text;
            }
        }

        private void btnGetTransitionDuration_Click(object sender, EventArgs e)
        {
            tbTransitionDuration.Value = _obs.Api.GetCurrentTransition().Duration;
        }

        private void btnSetTransitionDuration_Click(object sender, EventArgs e)
        {
            _obs.Api.SetTransitionDuration((int)tbTransitionDuration.Value);
        }
    }
}