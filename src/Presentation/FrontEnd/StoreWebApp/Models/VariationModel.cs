using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Virto.Helpers;

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
        private readonly ItemRelation[] _relations;
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
        /// <param name="relations">The relations.</param>
        /// <param name="selections">The selections.</param>
        /// <param name="selectedVariation">The selected variation.</param>
        public VariationsModel(ItemRelation[] relations, IEnumerable<string> selections, Item selectedVariation)
        {
            _relations = relations;

            //Create dictionary for filters if needed
            if (selectedVariation != null)
            {
                _selectedVariationCandidates.Add(selectedVariation.ItemId);

                //Create selections
                _selectedVariations = new Dictionary<string, SelectionValue>();
                var relationGroups = relations.Select(rel => rel.GroupName).Distinct();
                foreach (var prop in selectedVariation.ItemPropertyValues.LocalizedProperties().Where(p => relationGroups.Contains(p.Name)))
                {
                    _selectedVariations.Add(prop.Name, new SelectionValue { Value = prop.ToString() });
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
            foreach (var relationGroup in relations.GroupBy(v => v.GroupName))
            {
                var grp = new VariationGroup(relationGroup.Key);
                _variations.Add(grp);

                //Iterate through each ItemRelation where child item is sku
                foreach (var relation in relationGroup.Where(rg => rg.ChildItem is Sku).OrderBy(a => a.Priority))
                {
                    //Iterate through each property in ItemRelation ChildItem
                    var @group = relationGroup;
                    foreach (var prop in relation.ChildItem.ItemPropertyValues.LocalizedProperties().Where(p => p.Name == @group.Key))
                    {
                        var propValue = prop.ToString();
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
                                    sel.Key.Equals(relationGroup.Key, StringComparison.OrdinalIgnoreCase) ||
                                    _selectedVariations.ContainsKey(relationGroup.Key) &&
                                    _selectedVariations[relationGroup.Key].IsTrigger)
                                {
                                    //Do nor filter cleared group
                                    //Do not filter current property group
                                    //Do not filter trigger
                                    continue;
                                }
                                var selProp =
                                    relation.ChildItem.ItemPropertyValues.LocalizedProperties().FirstOrDefault(p => string.Equals(p.Name, sel.Key, StringComparison.InvariantCultureIgnoreCase));
                                if (selProp == null || GetText(selProp.ToString()).Equals(GetText(sel.Value.Value)))
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
                        if (_selectedVariations != null && _selectedVariations.ContainsKey(relationGroup.Key)
                            &&
                            _selectedVariations[relationGroup.Key].Value.Equals(propValue,
                                                                                StringComparison.OrdinalIgnoreCase))
                        {
                            selectItem.Selected = true;
                            grp.Value = propValue;

                            //remember candidate
                            if (!_selectedVariationCandidates.Contains(relation.ChildItemId))
                            {
                                _selectedVariationCandidates.Add(relation.ChildItemId);
                            }
                        }

                        grp.Items.Add(selectItem);
                    } //end foreach (var prop in relation
                }

                //if filter is explicitly cleared (set to null) select it
                if (_selectedVariations != null && _selectedVariations.ContainsKey(relationGroup.Key)
                    &&
                    string.Equals(_selectedVariations[relationGroup.Key].Value, "null",
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
                                       let item = relations.Select(r => r.ChildItem).First(i => i.ItemId == itemId)
                                       where item.ItemPropertyValues.LocalizedProperties().Any(p => this[p.Name] != null && !string.Equals(this[p.Name].Value, p.ToString()))
                                       select itemId)
                {
                    _selectedVariationCandidates.Remove(itemId);
                }
            }
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
        /// Gets the parent item identifier.
        /// </summary>
        /// <value>The parent item identifier.</value>
        public string ParentItemId
        {
            get { return _relations != null && _relations.Length > 0 ? _relations.First().ParentItemId : ""; }
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

        public CatalogItemWithPriceModel CatalogItem
        {
            get
            {
                return string.IsNullOrEmpty(SelectedVariationId) ? null : CatalogHelper.CreateCatalogModel(SelectedVariationId, ParentItemId, forcedActive: true);
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