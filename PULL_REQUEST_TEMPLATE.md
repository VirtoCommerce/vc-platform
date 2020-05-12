### Problem
A clear and concise description of what the problem is. Ex. I'm always frustrated when [...]

### Solution
A clear and concise description of what you want to happen.

### Proposed of changes
Describe the technical details of realization provided by you.

### Additional context (optional)
Add any other context or screenshots about the feature request here.

### Make sure these boxes are checked:
- [ ] Check all the changes in github PR - files count (non of them are redundant, have meaningful changes, all are added), if target branch is correct
- [ ] Check methods and variable namings - it should be self descriptive, no typos
- [ ] Check you did not introduce breaking changes in API and public models/services.
- [ ] Respect extensibility - https://community.virtocommerce.com/t/extensibility-basics-the-domain-model-and-persistence-layer-extension/141
- [ ] Follow [DRY](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself) and [SOLID](https://en.wikipedia.org/wiki/SOLID) principles
- [ ] For unit tests - follow ms best practices: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
- [ ] Consolidate solution dependencies in case you are using newer version, update VC module dependencies in module.manifest. Do not upgrade 3rd party packages that are shipped with the platform with the version newer than in the platform.
- [ ] Check code style conventions - https://github.com/VirtoCommerce/styleguide/blob/master/csharp.md
- [ ] Check PR have a concise and descriptive title, follow [git commit message rules](https://github.com/VirtoCommerce/styleguide/blob/master/gitcommits.md)
