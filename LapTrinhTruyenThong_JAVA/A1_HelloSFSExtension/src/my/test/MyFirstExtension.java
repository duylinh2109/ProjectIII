package my.test;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class MyFirstExtension extends SFSExtension {

	@Override
	public void init() {
		// TODO Auto-generated method stub
		trace("Hello! This is my first sfs extension");
	}
	
	
	
}
