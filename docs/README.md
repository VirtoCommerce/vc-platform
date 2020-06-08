# Overview

## What Is MkDocs?

MkDocs is a **fast**, **simple** and **downright gorgeous** static site generator that's geared towards building project documentation.
More info: https://www.mkdocs.org/

## Setup on-premises

1. Follow the steps of [Manual Installation](https://www.mkdocs.org/#manual-installation). Expected result: command `mkdocs --version` prints the version.
1. Run:
```
pip install mkdocs-awesome-pages-plugin
pip install mkdocs-git-revision-date-localized-plugin
pip install mkdocs-material
pip install mkdocs-minify-plugin
pip install mkdocs-redirects
pip install pymdown-extensions                    
```

## Serve (run)
1. Run:
```
mkdocs serve
```
1. Open http://127.0.0.1:8000

Troubleshooting
In case the new changes are not rendered, try shutting down the server (Ctrl+c) and starting again.
