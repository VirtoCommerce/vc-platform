using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Globalization;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Core.DataContracts;

namespace VirtoCommerce.Web.Models
{
    /// <summary>
    /// Class VariationsModel.
    /// </summary>
    public class VariationsModel
    {
        /// <summary>
        /// The _relations
        /// </summary>
        private readonly ProductVariation[] _availableVariations;
        /// <summary>
        /// The _selected variation candidates
        /// </summary>
        private readonly List<string> _selectedVariationCandidates = new List<string>();
        /// <summary>
        /// The _selected variations
        /// </summary>
        private readonly Dictionary<string, SelectionValue> _selectedVariations;

        /// <summary>
        /// The _variations
        /// </summary>
        private readonly List<VariationGroup> _variations = new List<VariationGroup>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VariationsModel"/> class.
        /// </summary>
        /// <param name="availableVariations">The available variations.</param>
        /// <param name="selections">The selections.</param>
        /// <param name="selectedVariation">The selected variation.</param>
        public VariationsModel(ProductVariation[] availableVariations, IEnumerable<string> selections, ProductVariation selectedVariation)
        {
            var relationGroups = GetPropertyGroups(availableVariations).ToArray();

            if (!relationGroups.Any())
            {
                return;
            }

            _availableVariations = availableVariations;


            //Create dictionary for filters if needed
            if (selectedVariation != null)
            {
                _selectedVariationCandidates.Add(selectedVariation.Id);

                //Create selections
                _selectedVariations = new Dictionary<string, SelectionValue>();
                foreach (var prop in selectedVariation.Properties.Where(p => relationGroups.Contains(p.Key)))
                {
                    _selectedVariations.Add(prop.Key, new SelectionValue { Value = prop.Value.ToString() });
                }
            }
            else if (selections != null)
            {
                _selectedVariations = new Dictionary<string, SelectionValue>();
                foreach (var sels in selections)
                {
                    foreach (var sel in sels.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var keyValue = sel.Split(':');
                        if (keyValue.Length >= 2)
                        {
                            _selectedVariations.Add(keyValue[0],
                                                    new SelectionValue
                                                    {
                                                        Value = keyValue[1],
                                                        IsTrigger =
                                                            keyValue.Length == 3 &&
                                                            string.Equals(keyValue[2], "t",
                                                                          StringComparison.OrdinalIgnoreCase)
                                                    });
                        }
                    }
                }
            }

            //Group item relations
            foreach (var relationGroup in relationGroups)
            {
                var grp = new VariationGroup(relationGroup);
                _variations.Add(grp);

                //Iterate through each ItemRelation where child item is sku
                foreach (var variation in availableVariations)
                {
                    //Iterate through each property in ItemRelation ChildItem
                    foreach (var prop in variation.Properties.Where(p => p.Key == relationGroup))
                    {
                        var propValue = prop.Value.ToString();
                        var name = GetText(propValue);

                        //make sure only distinct values are shown 
                        if (grp.Items.Any(i => i.Text == name))
                        {
                            continue;
                        }

                        //Filter by selections
                        var isAvailable = true;
                        if (_selectedVariations != null)
                        {
                            foreach (var sel in _selectedVariations)
                            {
                                if (string.IsNullOrEmpty(sel.Value.Value) ||
                                    string.Equals(sel.Value.Value, "null", StringComparison.OrdinalIgnoreCase) ||
                                    sel.Key.Equals(relationGroup, StringComparison.OrdinalIgnoreCase) ||
                                    _selectedVariations.ContainsKey(relationGroup) &&
                                    _selectedVariations[relationGroup].IsTrigger)
                                {
                                    //Do nor filter cleared group
                                    //Do not filter current property group
                                    //Do not filter trigger
                                    continue;
                                }
                                var selProp =
                                    variation.Properties.FirstOrDefault(p => string.Equals(p.Key, sel.Key, StringComparison.InvariantCultureIgnoreCase));
                                if (selProp.Equals(default(KeyValuePair<string,string[]>)) || GetText(selProp.Value.ToString()).Equals(GetText(sel.Value.Value)))
                                {
                                    continue;
                                }
                                isAvailable = false;
                                break;
                            }
                        }

                        if (!isAvailable)
                        {
                            continue;
                        }

                        var selectItem = new SelectListItem { Value = propValue, Text = name };

                        //Selected items
                        if (_selectedVariations != null && _selectedVariations.ContainsKey(relationGroup)
                            &&
                            _selectedVariations[relationGroup].Value.Equals(propValue, StringComparison.OrdinalIgnoreCase))
                        {
                            selectItem.Selected = true;
                            grp.Value = propValue;

                            //remember candidate
                            if (!_selectedVariationCandidates.Contains(variation.Id))
                            {
                                _selectedVariationCandidates.Add(variation.Id);
                            }
                        }

                        grp.Items.Add(selectItem);
                    } //end foreach (var prop in relation
                }

                //if filter is explicitly cleared (set to null) select it
                if (_selectedVariations != null && _selectedVariations.ContainsKey(relationGroup)
                    &&
                    string.Equals(_selectedVariations[relationGroup].Value, "null",
                                  StringComparison.OrdinalIgnoreCase))
                {
                    grp.Items[0].Selected = true;
                }
                //Automatically select last remaining choice 
                else if (grp.Items.Count == 2)
                {
                    grp.Items[1].Selected = true;
                    grp.Value = grp.Items[1].Value;
                }
            }

            if (!_variations.All(v => v.Items.Any(i => i.Selected)))
            {
                //Remove all candidates because there is undefined selection
                _selectedVariationCandidates.Clear();
            }
            else
            {
                //Extra filter by all properties
                foreach (var itemId in from itemId in _selectedVariationCandidates.ToArray()
                                       let item = availableVariations.First(i => i.Id == itemId)
                                       where item.Properties.Any(p => this[p.Key] != null && !string.Equals(this[p.Key].Value, p.Value.ToString()))
                                       select itemId)
                {
                    _selectedVariationCandidates.Remove(itemId);
                }
            }
        }

        private IEnumerable<string> GetPropertyGroups(ProductVariation[] variations)
        {
            var allNames = variations.SelectMany(x => x.Properties).Select(p => p.Key).ToArray();

            //We need to return only those properties where all variations have it
            return allNames.Where(x => variations.All(v => v.Properties.ContainsKey(x))).Distinct();
        }

        /// <summary>
        /// Property values can be defined using name|code pair seprated by pipe (|)
        /// Ex. color can be specified using Green|#00FF00 in english and Grün|#00FF00 in german
        /// </summary>
        /// <param name="val">return first part of value before pipe</param>
        /// <returns></returns>
        private string GetText(string val)
        {
            return val.Split('|')[0];
        }

        /// <summary>
        /// Gets the variations.
        /// </summary>
        /// <value>The variations.</value>
        public List<VariationGroup> Variations
        {
            get { return _variations; }
        }

        /// <summary>
        /// Gets the selected variation identifier.
        /// </summary>
        /// <value>The selected variation identifier.</value>
        public string SelectedVariationId
        {
            get
            {
                return _selectedVariationCandidates.Count == 1 ? _selectedVariationCandidates.First() : null;
            }
        }

        /// <summary>
        /// Gets the parent item identifier.
        /// </summary>
        /// <value>The parent item identifier.</value>
        public string ParentItemId
        {
            get { return _availableVariations != null && _availableVariations.Length > 0 ? _availableVariations.First().MainProductId : ""; }
        }

        public ItemModel CatalogItem
        {
            get
            {
                return string.IsNullOrEmpty(SelectedVariationId) ? null : _availableVariations.FirstOrDefault(x=>x.Id == SelectedVariationId).ToWebModel();
            }
        }

        /// <summary>
        /// Gets the <see cref="VariationGroup"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>VariationGroup.</returns>
        public VariationGroup this[string name]
        {
            get { return _variations.FirstOrDefault(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); }
        }

        /// <summary>
        /// Struct SelectionValue
        /// </summary>
        private struct SelectionValue
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            public string Value { get; set; }
            /// <summary>
            /// Gets or sets a value indicating whether this instance is trigger.
            /// </summary>
            /// <value><c>true</c> if this instance is trigger; otherwise, <c>false</c>.</value>
            public bool IsTrigger { get; set; }
        }
    }

    /// <summary>
    /// Class VariationGroup.
    /// </summary>
    public class VariationGroup
    {
        /// <summary>
        /// The items
        /// </summary>
        public List<SelectListItem> Items = new List<SelectListItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VariationGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public VariationGroup(string name)
        {
            //Add empty item
            Items.Add(new SelectListItem { Text = "Pick {0}".Localize((object)name), Value = "" });
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Required(ErrorMessage = "You must select this value")]
        public string Value { get; set; }
    }
}