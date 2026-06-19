# New API Migrator

A mod that lets old mods built for the legacy API keep running on the new version of the game.

## Why

When the game updates, its internal interfaces change, breaking many old mods. 
This mod automatically patches old mods on game startup so they can keep working without being remade.

## Limitations

This mod is **not** a magic bullet. 
It can only handle "simple renaming" types of issues. 
It **CANNOT** fix:

- Deep logic changes where the game behaves fundamentally differently
- Mods that use very low-level techniques
- Mods that rely on specific old-version behaviors that no longer exist

## License

MIT License © 2026 DreamBoxSpy, HKLab
