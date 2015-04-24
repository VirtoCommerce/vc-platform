using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Omu.ValueInjecter;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VirtoCommerce.Platform.Data.Common
{
	[CLSCompliant(false)]
	public class CloneInjection : ConventionInjection
	{
		private List<ObjectHistory> _createdObjects = null;

		public CloneInjection()
		{
			_createdObjects = new List<ObjectHistory>();
		}

		private class ObjectHistory
		{
			public object Source { get; set; }

			public object Target { get; set; }

			public ObjectHistory(object source, object target)
			{
				this.Source = source;

				PropertyInfo props = this.SourceType.GetProperties().AsEnumerable().
							 FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
			    if (props != null)
			    {
			        SourceId = props.GetValue(this.Source, null).ToString();
			        KeyPropertyName = props.Name;
			    }
			    this.Target = target;
			}

			public string SourceId { get; set; }

			public string KeyPropertyName { get; set; }

			//public string TargetId { get { return this.Target.Id; } }

			public Type SourceType { get { return this.Source.GetType(); } }
		}

		protected override void Inject(object source, object target)
		{
			var sourceProps = source.GetProps();
			var targetProps = target.GetProps();

			var ci = new ConventionInfo
			{
				Source =
				{
					Type = source.GetType(),
					Value = source
				},
				Target =
				{
					Type = target.GetType(),
					Value = target
				}
			};

			for (var i = 0; i < sourceProps.Count; i++)
			{
				var s = sourceProps[i];
				if (!s.IsReadOnly || (s.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))))
				{
					ci.SourceProp.Name = s.Name;
					ci.SourceProp.Value = s.GetValue(source);
					if (s.GetValue(source) != null)
						ci.SourceProp.Type = s.GetValue(source).GetType();
					else
						ci.SourceProp.Type = s.PropertyType;

					for (var j = 0; j < targetProps.Count; j++)
					{
						var t = targetProps[j];
						if (!t.IsReadOnly || (t.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))))
						{
							ci.TargetProp.Name = t.Name;
							ci.TargetProp.Value = t.GetValue(target);
							if (t.GetValue(target) != null)
								ci.TargetProp.Type = t.GetValue(target).GetType();
							else if (ci.SourceProp.Type != null)
								ci.TargetProp.Type = ci.SourceProp.Type;
							else
								ci.TargetProp.Type = t.PropertyType;
							if (Match(ci))
							{
								if (ci.SourceProp.Value != null)
								{
									var sv = SetValue(ci);
									if (sv != null)
										t.SetValue(target, sv);
								}
								break;
							}
						}
					}
				}
			}
		}

		protected override bool Match(ConventionInfo c)
		{
			bool propertyMatch = c.SourceProp.Name == c.TargetProp.Name;
			//bool sourceNotNull = c.SourceProp.Value != null;

			//bool targetPropertyIdWritable = false;
            //string keyPropName = c.Source.Type.GetProperties().AsEnumerable().
            //                 FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).Name;
			//if (propertyMatch && c.TargetProp.Name == keyPropName)
			//	targetPropertyIdWritable = true;

			return propertyMatch;//&& targetPropertyIdWritable;
		}

		private void AddObjectHistory(object source, object target)
		{
			if (source != null && target != null)
			{
				object actualSource = source as object;
				object actualTarget = target as object;

				if (!this.Exist(actualSource))
					_createdObjects.Add(new ObjectHistory(actualSource, actualTarget));
			}
		}

		private bool Exist(object source)
		{
			return this.FindHistoryObject(source) != null;
		}

		private ObjectHistory FindHistoryObject(object source)
		{
			ObjectHistory history = null;
			string sourceGuid = String.Empty;
			var tempGuid = source.GetType().GetProperties().AsEnumerable().
							 FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
			if (tempGuid != null)
				sourceGuid = tempGuid.GetValue(source, null).ToString();

			if (sourceGuid != String.Empty) // Find by id first if exist.
			{
				history = _createdObjects.FirstOrDefault(o => o.SourceId == sourceGuid);
			}
			else // If the Id is empty then use equality comparision.
			{
				// Attempt to find by object equality (GetHashCode).
				history = _createdObjects.FirstOrDefault(o => o.Source.GetHashCode() == source.GetHashCode());
			}

			return history;
		}

		private object GetTargetFromHistory(object source)
		{
			object target = null;

			if (source != null)
			{
				object actualSource = source;
				ObjectHistory history = this.FindHistoryObject(actualSource);

				if (history != null)
					target = history.Target;
			}

			return target;
		}

		protected override object SetValue(ConventionInfo c)
		{
			this.AddObjectHistory(c.Source.Value, c.Target.Value);

			//for value types and string just return the value as is
			if (c.SourceProp.Type.IsValueType || c.SourceProp.Type == typeof(string))
			{
				if ((c.TargetProp.Value == null) || (c.SourceProp.Value.GetHashCode() != c.TargetProp.Value.GetHashCode()))
					return c.SourceProp.Value;
				else
					return null;
			}

			//handle arrays
			if (c.SourceProp.Type.IsArray)
			{
				var arr = c.SourceProp.Value as Array;
				var clone = arr.Clone() as Array;

				for (int index = 0; index < arr.Length; index++)
				{
					var a = arr.GetValue(index);
					if (a.GetType().IsValueType || a.GetType() == typeof(string)) continue;
					clone.SetValue(Activator.CreateInstance(a.GetType()).InjectFrom(this, a), index);
				}
				return clone;
			}

			if (c.SourceProp.Type.IsGenericType)
			{
				//handle IEnumerable<> also ICollection<> IList<> List<>
				if (c.SourceProp.Type.GetGenericTypeDefinition().GetInterfaces().Contains(typeof(IEnumerable)))
				{
					Type targetChildType = c.TargetProp.Type.GetGenericArguments()[0];
					if (targetChildType.IsValueType || targetChildType == typeof(string)) return c.SourceProp.Value;

					return this.AddCollection(c, targetChildType);
				}

				//unhandled generic type, you could also return null or throw
				return null;
			}

			//for simple object types create a new instace and apply the clone injection on it
			if (c.TargetProp.Type == typeof(System.Type))
				return c.SourceProp.Value;
			else
			{
				object target = this.GetTargetFromHistory(c.SourceProp.Value);

				if (target != null)
					return target;
				else
					return Activator.CreateInstance(c.SourceProp.Type)
						.InjectFrom(this, c.SourceProp.Value);
			}
		}

		private object AddCollection(ConventionInfo c, Type targetChildType)
		{
			var list = c.TargetProp.Value;

			Type targetCollectionInterface = c.TargetProp.Type.GetInterface("ICollection`1");

			this.DeleteFromTargetCollection(c, targetCollectionInterface, targetChildType, list);
			this.AddOrUpdateTargetCollection(c, targetCollectionInterface, targetChildType, list);

			return list;
		}

		private void AddOrUpdateTargetCollection(ConventionInfo c, Type targetCollectionInterface, Type targetChildType, object list)
		{
			var addMethod = targetCollectionInterface.GetMethod("Add");
			foreach (object sourceChild in c.SourceProp.Value as IEnumerable)
			{
				object child = null;


				string sourceChildGuid = sourceChild.GetType().GetProperties().AsEnumerable().
							 FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).GetValue(sourceChild, null).ToString();

				bool found = this.FindInList(list, sourceChildGuid) != null;

				if (sourceChildGuid == String.Empty || !found)
				{
					child = Activator.CreateInstance(sourceChild.GetType());
					child.InjectFrom(this, sourceChild);
					addMethod.Invoke(list, new[] { child });
				}
				else
				{
					child = this.FindInList(list, sourceChildGuid);
					child.InjectFrom(this, sourceChild);
				}


			}

		}

		private void DeleteFromTargetCollection(ConventionInfo c, Type targetCollectionInterface, Type targetChildType, object list)
		{
			IEnumerable sourceList = c.SourceProp.Value as IEnumerable;

			List<object> childrenToDelete = new List<object>();

			var removeMethod = targetCollectionInterface.GetMethod("Remove");
			foreach (object targetChild in list as IEnumerable)
			{
				string targetChildGuid = targetChild.GetType().GetProperties().AsEnumerable().
							 FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).GetValue(targetChild, null).ToString();

				bool found = this.FindInList(sourceList, targetChildGuid) != null;

				if (!found)
					childrenToDelete.Add(targetChild);
			}

			foreach (object child in childrenToDelete)
				removeMethod.Invoke(list, new[] { child });
		}

		private object FindInList(object list, string id)
		{
			object child = null;

			foreach (object current in list as IEnumerable)
			{
				string currentGuid = current.GetType().GetProperties().AsEnumerable().
								FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(KeyAttribute))).GetValue(current, null).ToString();

				if (currentGuid == id)
				{
					child = current;
					break;
				}
			}

			return child;
		}
	}
}
