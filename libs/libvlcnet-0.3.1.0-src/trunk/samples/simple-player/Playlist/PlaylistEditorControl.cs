// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimplePlayer.Playlist
{
    /// <summary>
    /// Control can be used as playlist editor region.
    /// </summary>
    public partial class PlaylistEditorControl : UserControl
    {
        private readonly Playlist playlist = new Playlist();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PlaylistEditorControl() {
            InitializeComponent();
        }

        /// <summary>
        /// Linked playlist.
        /// </summary>
        public Playlist Playlist {
            get {
                return (playlist);
            }
        }

        /// <summary>
        /// Initialization event handler.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            //
            listBoxItems.DataSource = playlist.Items;
            listBoxItems.DisplayMember = "DisplayTitle";
            listBoxItems.DoubleClick += ListBoxItemsOnDoubleClick;
            playlist.PropertyChanged += PlaylistOnPropertyChanged;
        }

        private void PlaylistOnPropertyChanged(object sender, PropertyChangedEventArgs args) {
            if (args.PropertyName == "CurrentIndex") {
                listBoxItems.Invalidate(true);
            }
        }

        private void ListBoxItemsOnDoubleClick(object sender, EventArgs args) {
            ListBox.SelectedIndexCollection indices = listBoxItems.SelectedIndices;
            if (indices.Count != 0) {
                playlist.CurrentIndex = indices[0];
                playlist.EnterOnItem();
            }
        }

        private void listBoxItems_DrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            //
            if ((playlist.Items.Count > e.Index) && (e.Index >= 0)) {
                Brush brush = playlist.CurrentIndex == e.Index ? Brushes.WhiteSmoke : Brushes.YellowGreen;
                //
                PlaylistItem item = playlist.Items[e.Index];
                if (item.IsError) {
                    brush = playlist.CurrentIndex == e.Index ? Brushes.Tomato : Brushes.Red;
                }
                //
                e.Graphics.DrawString(item.DisplayTitle, DefaultFont, brush, new PointF(0,
                    listBoxItems.GetItemHeight(e.Index) * e.Index));
            }
        }

        private void listBoxItems_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                ArrayList indices = new ArrayList(listBoxItems.SelectedIndices);
                int count = indices.Count;
                if (count != 0) {
                    indices.Sort();
                    //
                    for (int i = 0; i < count; i++) {
                        int indexToRemove = (int) indices[i] - i;
                        if (indexToRemove == playlist.CurrentIndex) {
                            playlist.CurrentIndex = 0;
                        } else {
                            if (indexToRemove < playlist.CurrentIndex) {
                                playlist.CurrentIndex--;
                            }
                        }
                        //
                        playlist.Items.RemoveAt(indexToRemove);
                    }
                }
            }
        }
    }
}